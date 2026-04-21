using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyAppLib.Helpers;

public static class JsonOptions
{
    public static readonly JsonSerializerOptions Default = Create(false);
    public static readonly JsonSerializerOptions Pretty = Create(true);

    /// <summary>
    /// Json Serialize/Deserialize Options
    /// </summary>
    /// <param name="pretty">WriteIndented = pretty(bool)</param>
    /// <returns>JsonSerializerOptions</returns>
    public static JsonSerializerOptions Create(bool pretty)
    {
        return new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = pretty
        };
    }
}