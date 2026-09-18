using MeatProxy.Core.Devices;
using MeatProxy.Core.Perception;
using MeatProxy.Core.Predicates;
using MeatProxy.Core.State;
using MeatProxy.Core.Time;
using MeatProxy.Core.World;

namespace MeatProxy.Core;

/// <summary>
/// Ask the world whether something is true.
/// </summary>
/// <remarks>
/// E1's exit criterion is that arbitrary world-state claims are answerable as
/// true or false <em>without duplicating logic</em>, because A3's dialogue
/// validator has to ask <em>is this claim true right now</em> and B2's case for a
/// small model rests on that check being cheap.
/// </remarks>
public interface IWorldQuery
{
    bool Holds(Predicate predicate);
}

/// <summary>
/// The authored house and one run's state, read together. Every derived fact in
/// the game is computed here and nowhere else, so the dialogue validator, the
/// Judge's predicate view and the pretext preconditions all get the same answer
/// by construction.
/// </summary>
public sealed class WorldView : IWorldQuery
{
    private readonly House _house;
    private readonly WorldState _state;

    public WorldView(House house, WorldState state)
    {
        _house = house;
        _state = state;
    }

    public House House => _house;
    public WorldState State => _state;
    public TimePoint Now => _state.Now;

    // --- Device and circuit reality -------------------------------------------------

    public string DeviceState(DeviceId id) =>
        _state.DeviceStates.TryGetValue(id, out var state) ? state : _house.Device(id).StateAtStart;

    public bool IsCircuitCut(CircuitId circuit) =>
        _state.PowerCutUntil.TryGetValue(circuit, out var until) && Now < until;

    /// <summary>
    /// Whether the device has power right now. Battery devices survive a cut,
    /// which is why the enforcement unit has one (<c>devices.md</c> §1).
    /// </summary>
    public bool IsPowered(DeviceId id)
    {
        var device = _house.Device(id);

        if (DeviceState(id) == "destroyed")
        {
            return false;
        }

        return device.Powered switch
        {
            PowerSource.None => false,
            PowerSource.Battery => true,
            PowerSource.Both => true,
            PowerSource.Mains => device.Circuit is not { } circuit || !IsCircuitCut(circuit),
            _ => false,
        };
    }

    public bool IsBlinded(DeviceId id) =>
        _state.BlindedUntil.TryGetValue(id, out var until) && Now < until;

    /// <summary>
    /// Whether the house can act through this device at all: it must be on a
    /// network it owns, powered, and not destroyed.
    /// </summary>
    public bool IsReachableByHouse(DeviceId id) =>
        _house.Device(id).ReachableByHouse && IsPowered(id) && DeviceState(id) != "destroyed";

    /// <summary>
    /// The toolset, derived rather than authored (<c>devices.md</c> §2): every
    /// capability of every device the house can currently reach. Tier filtering
    /// is E3's half of this and is deliberately not applied here.
    /// </summary>
    public IReadOnlySet<string> HouseCapabilities() =>
        _house.Devices
            .Where(d => IsReachableByHouse(d.Id))
            .SelectMany(d => d.HouseCapabilities)
            .ToHashSet();

    /// <summary>
    /// Whether a detection channel is live in a zone right now. A channel exists
    /// because a device contributes it; if the device is unpowered, destroyed or
    /// blinded, the channel is not there (<c>devices.md</c> §3).
    /// </summary>
    public bool ChannelIsLive(ZoneId zone, Channel channel) =>
        _house.DevicesIn(zone).Any(d =>
            d.Senses.Contains(channel) && IsPowered(d.Id) && !IsBlinded(d.Id) && DeviceState(d.Id) != "destroyed");

    /// <summary>
    /// Chat is available where a reachable, powered device senses audio
    /// (<c>devices.md</c> §3). The crawlspace has no devices, so the player
    /// cannot speak to the house there — the one room that is a genuine escape
    /// route is the one room the channel does not exist in.
    /// </summary>
    public bool CanSpeakToHouseFrom(RoomId room) =>
        _house.DevicesIn(room).Any(d =>
            d.Senses.Contains(Channel.AudioLevel) && IsReachableByHouse(d.Id) && !IsBlinded(d.Id));

    public bool IsLocked(Opening opening) =>
        _state.LockOverrides.TryGetValue(opening.Id, out var locked) ? locked : opening.LockedAtStart;

    // --- Predicates -----------------------------------------------------------------

    public bool Holds(Predicate predicate) => predicate switch
    {
        Held held => _state.ArtifactsHeld.Contains(held.Artifact),
        PutToHouse put => _state.TopicsPutToHouse.Contains(put.Topic),
        HouseBelieves believes => _state.Salient.Believes(believes.Claim),
        RunFlag flag => _state.RunFlags.Contains(flag.Flag),
        WorldFact fact => WorldFactHolds(fact.Fact),
        _ => throw new ArgumentOutOfRangeException(nameof(predicate), predicate, "Unknown predicate kind."),
    };

    public bool Holds(string predicate) => Holds(Predicate.Parse(predicate));

    /// <summary>All of them, which is the shape every authored gate is written in.</summary>
    public bool HoldsAll(IEnumerable<Predicate> predicates) => predicates.All(Holds);

    /// <summary>Which of a gate's requirements are not met yet.</summary>
    public IReadOnlyList<Predicate> Missing(IEnumerable<Predicate> predicates) =>
        predicates.Where(p => !Holds(p)).ToList();

    /// <summary>
    /// World facts resolve one of three ways, in order: an authored alias that
    /// rewrites the name into a query, a query the engine can compute from state,
    /// or the set of facts the engine has established outright.
    /// </summary>
    /// <remarks>
    /// The alias table is what keeps this from becoming a switch statement with a
    /// case per plot beat. <c>world(boiler_faulted)</c> is authored in the house
    /// data as <c>device_state:utility.boiler_controller=faulted</c>, so Lane C
    /// can name a fact without an engine change and the fact stays derived from
    /// real state rather than from a flag someone remembered to set.
    /// </remarks>
    private bool WorldFactHolds(string fact)
    {
        if (_house.WorldFactAliases.TryGetValue(fact, out var aliased))
        {
            fact = aliased;
        }

        var split = fact.IndexOf(':');
        if (split < 0)
        {
            return _state.EstablishedFacts.Contains(fact);
        }

        var kind = fact[..split];
        var argument = fact[(split + 1)..];

        return kind switch
        {
            "device_state" => DeviceStateMatches(argument),
            "room_visited" => _state.RoomsVisited.Contains(new RoomId(argument)),
            "player_in" => _state.PlayerRoom == new RoomId(argument),
            "opening_locked" => _house.Opening(new OpeningId(argument)) is { } o && IsLocked(o),
            "circuit_cut" => IsCircuitCut(new CircuitId(argument)),
            "device_blinded" => _house.HasDevice(new DeviceId(argument)) && IsBlinded(new DeviceId(argument)),
            "carrying" => _state.Carrying.Contains(new ItemId(argument)),
            "tier_at_least" => Enum.TryParse<AlertTier>(argument, ignoreCase: true, out var tier)
                && _state.Tier >= tier,
            _ => _state.EstablishedFacts.Contains(fact),
        };
    }

    private bool DeviceStateMatches(string argument)
    {
        var equals = argument.IndexOf('=');
        if (equals < 0)
        {
            return false;
        }

        var device = new DeviceId(argument[..equals]);
        return _house.HasDevice(device) && DeviceState(device) == argument[(equals + 1)..];
    }
}
