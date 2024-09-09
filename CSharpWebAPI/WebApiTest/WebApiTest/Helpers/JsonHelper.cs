using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace WebApiTest.Helpers;

public static class JsonHelper
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = null
    };

    private static readonly JsonSerializerOptions OptionsEsc = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = null
    };

    private static readonly JsonSerializerOptions OptionsPretty = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public static string ModelToJson<T>(T modelClass, bool isEscapes = false) where T : class, new()
    {
        try
        {
            string result = JsonSerializer.Serialize(modelClass, isEscapes ? OptionsEsc : Options);
            return !string.IsNullOrWhiteSpace(result) ? result : string.Empty;
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"ModelToJson Error : {ex.Message}");
            return string.Empty;
        }
    }

    public static T JsonToModel<T>(string jsonString, bool isEscapes = false) where T : class, new()
    {
        try
        {
            if (!IsJsonValid(jsonString))
            {
                LogHelper.Logger.Warn($"JsonToModel Warn : Input JSON value : {jsonString}");
                return new T();
            }

            T? result = JsonSerializer.Deserialize<T>(jsonString, isEscapes ? OptionsEsc : Options);
            return result ?? new T();
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"JsonToModel Error : {ex.Message}");
            return new T();
        }
    }

    public static string JsonPretty(this string jsonString)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return jsonString;
            }

            using JsonDocument jDoc = JsonDocument.Parse(jsonString);

            return JsonSerializer.Serialize(jDoc, OptionsPretty);
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"JsonPretty Error : {ex.Message}");
            return string.Empty;
        }
    }

    public static bool IsJsonValid(string jsonString)
    {
        JsonDocument? jDoc = null;

        try
        {
            Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(jsonString));
            return JsonDocument.TryParseValue(ref reader, out jDoc);
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"IsJsonValid Error : {ex.Message}");
            return false;
        }
        finally
        {
            jDoc?.Dispose();
        }
    }
}