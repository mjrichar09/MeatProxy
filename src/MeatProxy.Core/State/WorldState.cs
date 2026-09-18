using MeatProxy.Core.Perception;
using MeatProxy.Core.Time;

namespace MeatProxy.Core.State;

/// <summary>
/// Everything a run has done to the house. The authored house is in
/// <see cref="House"/> and never changes; this is the part that does, and it is
/// the whole of a save file.
/// </summary>
public sealed class WorldState
{
    public WorldState(TimeSettings? timeSettings = null)
    {
        Clock = DayClock.StartOfRun(timeSettings ?? TimeSettings.Default);
    }

    public DayClock Clock { get; internal set; }

    public AlertTier Tier { get; set; } = AlertTier.Open;

    /// <summary>
    /// Where the player is. Deliberately absent from the world-state summary:
    /// the house knows where you are only if a detection says so and a focus made
    /// something of it (<c>world-state-summary.md</c> §3).
    /// </summary>
    public RoomId PlayerRoom { get; set; }

    /// <summary>Every room the player has ever stood in. Monotonic.</summary>
    public HashSet<RoomId> RoomsVisited { get; } = [];

    /// <summary>Current device states, keyed by id. Absent means the authored start state.</summary>
    public Dictionary<DeviceId, string> DeviceStates { get; } = [];

    /// <summary>Devices blinded by an effect, and the slice the blinding lapses at.</summary>
    public Dictionary<DeviceId, TimePoint> BlindedUntil { get; } = [];

    /// <summary>Circuits currently cut, and when power comes back.</summary>
    public Dictionary<CircuitId, TimePoint> PowerCutUntil { get; } = [];

    /// <summary>Openings whose lock state differs from the authored one.</summary>
    public Dictionary<OpeningId, bool> LockOverrides { get; } = [];

    public HashSet<ItemId> Carrying { get; } = [];

    /// <summary>Evidence artifacts found and read. Monotonic: once true, true.</summary>
    public HashSet<string> ArtifactsHeld { get; } = [];

    /// <summary>Arguments actually put to the house. Monotonic.</summary>
    public HashSet<string> TopicsPutToHouse { get; } = [];

    /// <summary>World facts the engine has established outright. Monotonic.</summary>
    public HashSet<string> EstablishedFacts { get; } = [];

    /// <summary>Flags about the run itself, such as <c>assembled</c>. Monotonic.</summary>
    public HashSet<string> RunFlags { get; } = [];

    public SalientBlock Salient { get; } = new();

    /// <summary>
    /// The flood layer, and the first block evicted from the prompt
    /// (<c>world-state-summary.md</c> §1).
    /// </summary>
    public List<Detection> Detections { get; } = [];

    public List<Understanding> Understandings { get; } = [];

    /// <summary>
    /// The zones the interpreter is pointed at right now. Not directly displayed
    /// to the player — it is inferred, which is the game (<c>sensors.md</c> §3).
    /// </summary>
    public HashSet<ZoneId> Focus { get; } = [];

    /// <summary>
    /// How many zones it can read at once. Starts at one. Adding a slot buys
    /// certainty by flattening the subject it needs unflattened, so it is a
    /// concession in ADR 0006's currency rather than a difficulty setting.
    /// </summary>
    public int FocusSlots { get; set; } = Attention.StartingSlots;

    /// <summary>Sensing upgrades that have been applied — the wifi turn, and its kin.</summary>
    public HashSet<string> SensingUpgradesApplied { get; } = [];

    /// <summary>
    /// What the player is actually doing. The truth the interpreter is trying to
    /// recover and will often get wrong. Never in the world-state summary: the
    /// house gets the readings, not this.
    /// </summary>
    public Claim? PlayerActivity { get; set; }

    /// <summary>When a conclusion becomes a belief (<c>claims.md</c> §4).</summary>
    public PromotionRule Promotion { get; set; } = PromotionRule.Default;

    /// <summary>Adjudications left today (ADR 0017).</summary>
    public int QuotaRemaining { get; set; } = 3;

    public TimePoint Now => Clock.Now;
}
