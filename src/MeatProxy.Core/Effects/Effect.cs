using System.Text.Json.Serialization;
using MeatProxy.Core.Perception;

namespace MeatProxy.Core.Effects;

/// <summary>Three buckets, never numbers (<c>effects.md</c> §1).</summary>
public enum Severity
{
    Low,
    Medium,
    High,
}

/// <summary>
/// The closed typed-effect vocabulary (<c>effects.md</c>). Thirteen types, and
/// that is the whole list.
/// </summary>
/// <remarks>
/// <para><em>The model proposes; the engine disposes.</em></para>
/// <para>
/// Every model adjudication returns effects from this set and nothing else. The
/// model never mutates state, never writes prose that becomes true, and never
/// invents a type. Adding a member here invalidates every eval fixture Lane A
/// has written, so it is a D3 decision with a written rationale, not a patch.
/// </para>
/// <para>
/// What is absent is as load-bearing as what is present. Nothing here unlocks,
/// opens or grants passage; nothing moves tier, Clarity, quota or the 61%; and
/// nothing creates an object. Progression is never an adjudication outcome.
/// </para>
/// </remarks>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(NoEffect), "no_effect")]
[JsonDerivedType(typeof(CreateNoise), "create_noise")]
[JsonDerivedType(typeof(EmitOdor), "emit_odor")]
[JsonDerivedType(typeof(EmitHeat), "emit_heat")]
[JsonDerivedType(typeof(TripSensor), "trip_sensor")]
[JsonDerivedType(typeof(BlindSensor), "blind_sensor")]
[JsonDerivedType(typeof(DamageDevice), "damage_device")]
[JsonDerivedType(typeof(ChangeDeviceState), "change_device_state")]
[JsonDerivedType(typeof(CutPower), "cut_power")]
[JsonDerivedType(typeof(RevealObject), "reveal_object")]
[JsonDerivedType(typeof(MarkObject), "mark_object")]
[JsonDerivedType(typeof(ConsumeItem), "consume_item")]
[JsonDerivedType(typeof(HarmPlayer), "harm_player")]
public abstract record Effect;

/// <summary>Always legal, and a real answer.</summary>
public sealed record NoEffect(string Reason) : Effect;

/// <summary>Duration is in slices throughout.</summary>
public sealed record CreateNoise(ZoneId Zone, Magnitude Magnitude, int Duration) : Effect;

public sealed record EmitOdor(ZoneId Zone, Magnitude Magnitude, int Duration) : Effect;

public sealed record EmitHeat(ZoneId Zone, Magnitude Magnitude, int Duration) : Effect;

public sealed record TripSensor(DeviceId Device, Channel Channel) : Effect;

public sealed record BlindSensor(DeviceId Device, int Duration) : Effect;

public sealed record DamageDevice(DeviceId Device, Severity Severity) : Effect;

public sealed record ChangeDeviceState(DeviceId Device, string State) : Effect;

public sealed record CutPower(CircuitId Circuit, int Duration) : Effect;

/// <summary>Reveals to the <em>player</em>, not to the world.</summary>
public sealed record RevealObject(string Object) : Effect;

/// <summary>A durable delta (ADR 0015). The object's record sets the reversal cost.</summary>
public sealed record MarkObject(string Object, Magnitude Magnitude) : Effect;

public sealed record ConsumeItem(ItemId Item) : Effect;

/// <summary><see cref="Severity.High"/> is refused outside authored scenes.</summary>
public sealed record HarmPlayer(Severity Severity) : Effect;

/// <summary>
/// What the model returns (<c>effects.md</c> §2). <see cref="Rationale"/> is for
/// the log and for the player-facing line, and is <em>never</em> read back as
/// truth: if it claims something the effects do not do, the effects win.
/// </summary>
public sealed record Adjudication
{
    public IReadOnlyList<Effect> Effects { get; init; } = [];
    public string Rationale { get; init; } = string.Empty;
}
