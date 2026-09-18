using System.Text.Json;
using MeatProxy.Core.Perception;
using MeatProxy.Core.State;
using MeatProxy.Core.Time;

namespace MeatProxy.Core.Persistence;

/// <summary>
/// A save file: everything the run did, and nothing the house is.
/// </summary>
/// <remarks>
/// The authored house is reloaded from its data file on restore, so a save does
/// not carry a copy of the world and a content fix does not invalidate an
/// existing save. <see cref="HouseName"/> is recorded only so a save cannot be
/// restored against the wrong house.
/// </remarks>
public sealed record SaveGame
{
    public int Version { get; init; } = 1;
    public required string HouseName { get; init; }

    public required TimeSettings TimeSettings { get; init; }
    public int Day { get; init; }
    public int SlicesSpent { get; init; }
    public int Budget { get; init; }

    public AlertTier Tier { get; init; }
    public required RoomId PlayerRoom { get; init; }
    public int QuotaRemaining { get; init; }

    public IReadOnlyList<RoomId> RoomsVisited { get; init; } = [];
    public IReadOnlyDictionary<DeviceId, string> DeviceStates { get; init; } = new Dictionary<DeviceId, string>();
    public IReadOnlyDictionary<DeviceId, TimePoint> BlindedUntil { get; init; } = new Dictionary<DeviceId, TimePoint>();
    public IReadOnlyDictionary<CircuitId, TimePoint> PowerCutUntil { get; init; } = new Dictionary<CircuitId, TimePoint>();
    public IReadOnlyDictionary<OpeningId, bool> LockOverrides { get; init; } = new Dictionary<OpeningId, bool>();
    public IReadOnlyList<ItemId> Carrying { get; init; } = [];
    public IReadOnlyList<string> ArtifactsHeld { get; init; } = [];
    public IReadOnlyList<string> TopicsPutToHouse { get; init; } = [];
    public IReadOnlyList<string> EstablishedFacts { get; init; } = [];
    public IReadOnlyList<string> RunFlags { get; init; } = [];

    public int SalientCap { get; init; }
    public IReadOnlyList<SalientFact> Salient { get; init; } = [];

    public IReadOnlyList<Detection> Detections { get; init; } = [];
    public IReadOnlyList<Understanding> Understandings { get; init; } = [];

    public static SaveGame From(House house, WorldState state) => new()
    {
        HouseName = house.Name,
        TimeSettings = state.Clock.Settings,
        Day = state.Clock.Day,
        SlicesSpent = state.Clock.SlicesSpent,
        Budget = state.Clock.Budget,
        Tier = state.Tier,
        PlayerRoom = state.PlayerRoom,
        QuotaRemaining = state.QuotaRemaining,
        RoomsVisited = [.. state.RoomsVisited],
        DeviceStates = new Dictionary<DeviceId, string>(state.DeviceStates),
        BlindedUntil = new Dictionary<DeviceId, TimePoint>(state.BlindedUntil),
        PowerCutUntil = new Dictionary<CircuitId, TimePoint>(state.PowerCutUntil),
        LockOverrides = new Dictionary<OpeningId, bool>(state.LockOverrides),
        Carrying = [.. state.Carrying],
        ArtifactsHeld = [.. state.ArtifactsHeld],
        TopicsPutToHouse = [.. state.TopicsPutToHouse],
        EstablishedFacts = [.. state.EstablishedFacts],
        RunFlags = [.. state.RunFlags],
        SalientCap = state.Salient.Cap,
        Salient = [.. state.Salient.Facts],
        Detections = [.. state.Detections],
        Understandings = [.. state.Understandings],
    };

    public WorldState Restore(House house)
    {
        if (house.Name != HouseName)
        {
            throw new InvalidOperationException(
                $"This save is of '{HouseName}' and the house loaded is '{house.Name}'.");
        }

        var state = new WorldState(TimeSettings)
        {
            Clock = new DayClock(TimeSettings, Day, SlicesSpent, Budget),
            Tier = Tier,
            PlayerRoom = PlayerRoom,
            QuotaRemaining = QuotaRemaining,
        };

        state.RoomsVisited.UnionWith(RoomsVisited);
        state.Carrying.UnionWith(Carrying);
        state.ArtifactsHeld.UnionWith(ArtifactsHeld);
        state.TopicsPutToHouse.UnionWith(TopicsPutToHouse);
        state.EstablishedFacts.UnionWith(EstablishedFacts);
        state.RunFlags.UnionWith(RunFlags);

        foreach (var (device, deviceState) in DeviceStates)
        {
            state.DeviceStates[device] = deviceState;
        }

        foreach (var (device, until) in BlindedUntil)
        {
            state.BlindedUntil[device] = until;
        }

        foreach (var (circuit, until) in PowerCutUntil)
        {
            state.PowerCutUntil[circuit] = until;
        }

        foreach (var (opening, locked) in LockOverrides)
        {
            state.LockOverrides[opening] = locked;
        }

        state.Salient.Cap = SalientCap;
        state.Salient.Restore(Salient);
        state.Detections.AddRange(Detections);
        state.Understandings.AddRange(Understandings);

        return state;
    }

    public string ToJson() => JsonSerializer.Serialize(this, Json.Pretty);

    public static SaveGame FromJson(string json) =>
        JsonSerializer.Deserialize<SaveGame>(json, Json.Options)
        ?? throw new InvalidOperationException("Empty save file.");
}
