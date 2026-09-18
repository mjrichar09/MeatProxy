namespace MeatProxy.Core.Effects;

/// <summary>An effect that did not survive validation, and why.</summary>
public sealed record RejectedEffect(Effect Effect, string Reason);

/// <summary>
/// What survived (<c>effects.md</c> §3.5). A partially valid adjudication runs
/// its valid half.
/// </summary>
public sealed record ValidationResult
{
    public IReadOnlyList<Effect> Accepted { get; init; } = [];
    public IReadOnlyList<RejectedEffect> Rejected { get; init; } = [];

    /// <summary>
    /// Rejection is silent and in-fiction: the player sees <em>nothing
    /// happened</em>, which is already a legitimate outcome, so the seam never
    /// shows. This list is for the log and the debug view, never for a message.
    /// </summary>
    public bool AnythingHappened => Accepted.Any(e => e is not NoEffect);
}

/// <summary>
/// The engine's half of <em>the model proposes; the engine disposes</em>.
/// Validates a proposed adjudication against real world state before anything is
/// executed (<c>effects.md</c> §3).
/// </summary>
public sealed class EffectValidator
{
    /// <summary>At most four effects per adjudication (<c>effects.md</c> §3.4).</summary>
    public const int MaxEffects = 4;

    private readonly WorldView _world;
    private readonly Func<DeviceId, bool> _isAuthoredCritical;

    /// <param name="world">The house and the run, read together.</param>
    /// <param name="isAuthoredCritical">
    /// Which devices are authored-critical, for the severity downgrade in
    /// <c>effects.md</c> §3.4. <c>devices.md</c> carries no such field today, so
    /// this is supplied by the caller rather than invented here — see the note in
    /// <c>docs/schemas/README.md</c>. The default treats nothing as critical.
    /// </param>
    public EffectValidator(WorldView world, Func<DeviceId, bool>? isAuthoredCritical = null)
    {
        _world = world;
        _isAuthoredCritical = isAuthoredCritical ?? (_ => false);
    }

    public ValidationResult Validate(Adjudication adjudication)
    {
        var accepted = new List<Effect>();
        var rejected = new List<RejectedEffect>();
        var harmSeen = false;

        foreach (var effect in adjudication.Effects)
        {
            if (accepted.Count >= MaxEffects)
            {
                rejected.Add(new RejectedEffect(effect, $"Over the cap of {MaxEffects} effects."));
                continue;
            }

            if (effect is HarmPlayer)
            {
                if (harmSeen)
                {
                    rejected.Add(new RejectedEffect(effect, "At most one harm_player per adjudication."));
                    continue;
                }

                harmSeen = true;
            }

            var checkedEffect = Check(effect, out var reason);
            if (checkedEffect is null)
            {
                rejected.Add(new RejectedEffect(effect, reason!));
                continue;
            }

            accepted.Add(checkedEffect);
        }

        return new ValidationResult { Accepted = accepted, Rejected = rejected };
    }

    /// <summary>
    /// Returns the effect to execute — which may be a downgraded version of the
    /// one proposed — or null with a reason.
    /// </summary>
    private Effect? Check(Effect effect, out string? reason)
    {
        reason = null;

        switch (effect)
        {
            case NoEffect:
                return effect;

            case CreateNoise noise:
                return ZoneExists(noise.Zone, ref reason) ? effect : null;

            case EmitOdor odor:
                return ZoneExists(odor.Zone, ref reason) ? effect : null;

            case EmitHeat heat:
                return ZoneExists(heat.Zone, ref reason) ? effect : null;

            case TripSensor trip:
                if (!DeviceExists(trip.Device, ref reason))
                {
                    return null;
                }

                if (!_world.IsPowered(trip.Device))
                {
                    reason = $"Device '{trip.Device}' has no power.";
                    return null;
                }

                if (!_world.House.Device(trip.Device).Senses.Contains(trip.Channel))
                {
                    reason = $"Device '{trip.Device}' does not sense {trip.Channel}.";
                    return null;
                }

                return effect;

            case BlindSensor blind:
                return DeviceExists(blind.Device, ref reason) ? effect : null;

            case DamageDevice damage:
                if (!DeviceExists(damage.Device, ref reason))
                {
                    return null;
                }

                // High severity on an authored-critical device is downgraded,
                // not refused. Losing the beat is worse than losing the damage.
                return damage.Severity == Severity.High && _isAuthoredCritical(damage.Device)
                    ? damage with { Severity = Severity.Medium }
                    : effect;

            case ChangeDeviceState change:
                if (!DeviceExists(change.Device, ref reason))
                {
                    return null;
                }

                var device = _world.House.Device(change.Device);
                if (!device.States.Contains(change.State))
                {
                    reason = $"'{change.State}' is not one of '{change.Device}'s declared states.";
                    return null;
                }

                if (_world.DeviceState(change.Device) == change.State)
                {
                    reason = $"Device '{change.Device}' is already {change.State}.";
                    return null;
                }

                return effect;

            case CutPower cut:
                if (!_world.House.HasCircuit(cut.Circuit))
                {
                    reason = $"No circuit '{cut.Circuit}'.";
                    return null;
                }

                if (_world.IsCircuitCut(cut.Circuit))
                {
                    reason = $"Circuit '{cut.Circuit}' is already cut.";
                    return null;
                }

                return effect;

            case RevealObject reveal:
                return ObjectIsPresent(reveal.Object, ref reason) ? effect : null;

            case MarkObject mark:
                return ObjectIsPresent(mark.Object, ref reason) ? effect : null;

            case ConsumeItem consume:
                if (!_world.State.Carrying.Contains(consume.Item))
                {
                    reason = $"Item '{consume.Item}' is not held.";
                    return null;
                }

                return effect;

            case HarmPlayer harm:
                if (harm.Severity == Severity.High)
                {
                    reason = "harm_player(high) is refused outside authored scenes.";
                    return null;
                }

                return effect;

            default:
                // Unreachable while the hierarchy stays closed, which is the
                // point of it being closed.
                reason = $"'{effect.GetType().Name}' is not in the effect vocabulary.";
                return null;
        }
    }

    private bool ZoneExists(ZoneId zone, ref string? reason)
    {
        if (_world.House.HasZone(zone))
        {
            return true;
        }

        reason = $"No zone '{zone}'.";
        return false;
    }

    private bool DeviceExists(DeviceId device, ref string? reason)
    {
        if (_world.House.HasDevice(device))
        {
            return true;
        }

        reason = $"No device '{device}'.";
        return false;
    }

    /// <summary>
    /// Objects are fixtures or devices in the player's room. The house is
    /// authored, so nothing here may bring one into being.
    /// </summary>
    private bool ObjectIsPresent(string name, ref string? reason)
    {
        var room = _world.House.Room(_world.State.PlayerRoom);
        var present = room.Fixtures.Any(f => f.Id.Value == name)
            || _world.House.DevicesIn(room.Id).Any(d => d.Id.Value == name);

        if (present)
        {
            return true;
        }

        reason = $"'{name}' is not in {room.Id}.";
        return false;
    }
}
