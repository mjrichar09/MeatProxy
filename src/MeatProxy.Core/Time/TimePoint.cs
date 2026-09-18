namespace MeatProxy.Core.Time;

/// <summary>
/// A day and a slice inside it. Time is days, and days are slices (ADR 0008,
/// <c>DESIGN.md</c> §5).
/// </summary>
public readonly record struct TimePoint(int Day, int Slice) : IComparable<TimePoint>
{
    public int CompareTo(TimePoint other) =>
        Day != other.Day ? Day.CompareTo(other.Day) : Slice.CompareTo(other.Slice);

    public static bool operator <(TimePoint a, TimePoint b) => a.CompareTo(b) < 0;
    public static bool operator >(TimePoint a, TimePoint b) => a.CompareTo(b) > 0;
    public static bool operator <=(TimePoint a, TimePoint b) => a.CompareTo(b) <= 0;
    public static bool operator >=(TimePoint a, TimePoint b) => a.CompareTo(b) >= 0;

    public override string ToString() => $"day {Day}, slice {Slice}";
}
