using System.Text.Json;
using System.Text.Json.Serialization;

namespace MeatProxy.Core;

/// <summary>
/// The identifier forms fixed by <c>docs/schemas/world.md</c> §1. Snake case,
/// stable for the life of the project, never reused after deletion.
/// </summary>
/// <remarks>
/// These are separate types rather than bare strings on purpose. A room id and
/// a zone id are both lowercase words with underscores in them, and the day a
/// device id is passed where a circuit id was meant is the day the schemas start
/// to drift. The compiler is cheaper than that bug.
/// </remarks>
public interface IIdentifier
{
    string Value { get; }
}

public readonly record struct RoomId(string Value) : IIdentifier
{
    /// <summary>
    /// The boundary. An opening whose far side is <see cref="Exterior"/> leaves
    /// the house, which is the thing the game is about (ADR 0030), so it is a
    /// real value here rather than a null.
    /// </summary>
    public static readonly RoomId Exterior = new("exterior");

    public bool IsExterior => Value == Exterior.Value;
    public override string ToString() => Value;
}

public readonly record struct LevelId(string Value) : IIdentifier
{
    public override string ToString() => Value;
}

public readonly record struct OpeningId(string Value) : IIdentifier
{
    public override string ToString() => Value;
}

public readonly record struct FixtureId(string Value) : IIdentifier
{
    public override string ToString() => Value;
}

public readonly record struct DeviceId(string Value) : IIdentifier
{
    /// <summary>The <c>&lt;room&gt;.&lt;name&gt;</c> form's room half.</summary>
    public RoomId Room => new(Value.Split('.', 2)[0]);

    public override string ToString() => Value;
}

public readonly record struct ZoneId(string Value) : IIdentifier
{
    public override string ToString() => Value;
}

public readonly record struct CircuitId(string Value) : IIdentifier
{
    public override string ToString() => Value;
}

public readonly record struct ItemId(string Value) : IIdentifier
{
    public override string ToString() => Value;
}

/// <summary>
/// Writes every <see cref="IIdentifier"/> as a bare JSON string, so the authored
/// data files read the way the schemas are written rather than the way the CLR
/// lays out a struct.
/// </summary>
public sealed class IdentifierJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert.IsValueType && typeof(IIdentifier).IsAssignableFrom(typeToConvert);

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) =>
        (JsonConverter)Activator.CreateInstance(
            typeof(IdentifierJsonConverter<>).MakeGenericType(typeToConvert))!;

    private sealed class IdentifierJsonConverter<T> : JsonConverter<T>
        where T : struct, IIdentifier
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString()
                ?? throw new JsonException($"A {typeof(T).Name} may not be null.");
            return (T)Activator.CreateInstance(typeof(T), value)!;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.Value);

        public override T ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            (T)Activator.CreateInstance(typeof(T), reader.GetString()!)!;

        public override void WriteAsPropertyName(Utf8JsonWriter writer, T value, JsonSerializerOptions options) =>
            writer.WritePropertyName(value.Value);
    }
}
