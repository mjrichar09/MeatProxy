using System.Text.Json;
using System.Text.Json.Serialization;

namespace MeatProxy.Core.Persistence;

/// <summary>
/// One set of serializer options for the whole core, so authored data files and
/// save files read the way the schemas are written: snake case throughout, enums
/// as their schema spellings, identifiers as bare strings.
/// </summary>
public static class Json
{
    public static JsonSerializerOptions Options { get; } = Build(indented: false);

    public static JsonSerializerOptions Pretty { get; } = Build(indented: true);

    private static JsonSerializerOptions Build(bool indented)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            PropertyNameCaseInsensitive = true,
            WriteIndented = indented,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        };

        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower));
        options.Converters.Add(new IdentifierJsonConverterFactory());
        return options;
    }
}
