using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyAppLib.Helpers;

public static class JsonOptions
{
    public static readonly JsonSerializerOptions Default = Create(false);
    public static readonly JsonSerializerOptions Pretty = Create(true);

    public static JsonSerializerOptions Create(bool pretty)
    {
        return new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = pretty
        };
    }
}