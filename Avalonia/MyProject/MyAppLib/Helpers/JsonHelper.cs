using System;
using System.Text.Json;

namespace MyAppLib.Helpers;

public interface IJsonConvertible
{
    // IJsonConvertible 상속받는 모델(Class)만 JsonHelper 사용 가능
}

public static class JsonHelper
{
    public static string ToJson<T>(this T source, bool pretty = false) where T : IJsonConvertible
    {
        try
        {
            if (source == null)
            {
                throw new JsonException("Source cannot be null");
            }

            return JsonSerializer.Serialize(
                source,
                pretty ? JsonOptions.Pretty : JsonOptions.Default);
        }
        catch (Exception ex)
        {
            throw new JsonException("Failed during serialization", ex);
        }
    }

    extension(string json)
    {
        public T FromJson<T>() where T : IJsonConvertible
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                {
                    throw new JsonException("Source cannot be null or empty");
                }

                return JsonSerializer.Deserialize<T>(json, JsonOptions.Default)
                       ?? throw new JsonException("Failed during deserialization");
            }
            catch (Exception ex)
            {
                throw new JsonException("Failed during deserialization", ex);
            }
        }

        public bool TryFromJson<T>(out T? result) where T : IJsonConvertible
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                {
                    LogHelper.Error("Source is null or empty.");
                    result = default;
                    return false;
                }

                result = JsonSerializer.Deserialize<T>(json, JsonOptions.Default);

                if (result is not null)
                {
                    return true;
                }

                LogHelper.Error("Failed while trying to deserialize");
                return false;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"Failed while trying to deserialize :  {ex.Message}");
                result = default;
                return false;
            }
        }
    }

    public static T DeepCopy<T>(this T source, bool pretty = false) where T : IJsonConvertible
    {
        try
        {
            if (source == null)
            {
                throw new JsonException("Source cannot be null");
            }

            JsonSerializerOptions options = pretty ? JsonOptions.Pretty : JsonOptions.Default;
            string json = JsonSerializer.Serialize(source, options);
            return JsonSerializer.Deserialize<T>(json, options)
                   ?? throw new JsonException("Failed during deserialization");
        }
        catch (Exception ex)
        {
            throw new JsonException("Failed during deserialization", ex);
        }
    }
}