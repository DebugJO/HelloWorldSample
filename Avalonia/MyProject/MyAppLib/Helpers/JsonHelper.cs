using System;
using System.Text.Json;

namespace MyAppLib.Helpers;

/// <summary>
/// IJsonConvertible 상속받는 모델(Class/Record)만 JsonHelper 사용 가능
/// <![CDATA[
/// string ToJson<T>(this T source, bool pretty = false)
/// T FromJson<T>()
/// bool TryFromJson<T>(out T? result)
///  T DeepCopy<T>(this T source, bool pretty = false)
/// ]]>
/// </summary>
public interface IJsonConvertible
{
    // IJsonConvertible 상속받는 모델(Class)만 JsonHelper 사용 가능
}

public static class JsonHelper
{
    /// <summary>
    /// Model Class/Record를 Json 문자열로 반환
    /// IJsonConvertible 상속받는 모델(Class/Record)만 해당
    /// </summary>
    /// <param name="source">Model Class/Record</param>
    /// <param name="pretty">WriteIndented = pretty</param>
    /// <typeparam name="T">IJsonConvertible</typeparam>
    /// <returns>string json</returns>
    /// <exception cref="JsonException">Failed during serialization</exception>
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
        /// <summary>
        /// Json 문자열을 Model Class/Record로 반환
        /// IJsonConvertible 상속받는 모델(Class/Record)만 해당
        /// </summary>
        /// <typeparam name="T">IJsonConvertible</typeparam>
        /// <returns>Model Class/Record</returns>
        /// <exception cref="JsonException">Failed during serialization</exception>
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

    /// <summary>
    ///  클래스객체를 DeepCopy 
    /// </summary>
    /// <param name="source">Model Class/Record</param>
    /// <param name="pretty">WriteIndented = pretty</param>
    /// <typeparam name="T">IJsonConvertible</typeparam>
    /// <returns>Model Class/Record</returns>
    /// <exception cref="JsonException">Failed during deserialization</exception>
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