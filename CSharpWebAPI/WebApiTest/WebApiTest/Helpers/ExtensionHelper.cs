using System;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebApiTest.Helpers;

public static partial class ExtensionHelper
{
    public static string TrimSafe(this string source)
    {
        return string.IsNullOrWhiteSpace(source) ? string.Empty : source.Trim();
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex MyRegex();

    public static string RemoveDoubleSpace(this string source)
    {
        return string.IsNullOrWhiteSpace(source) ? string.Empty : MyRegex().Replace(source, $"{(char)32}");
    }

    public static string ToSingleLine(this string source)
    {
        return string.IsNullOrWhiteSpace(source) ? string.Empty : source.Replace("\n", ((char)32).ToString()).Replace("\r", ((char)32).ToString()).RemoveDoubleSpace().TrimSafe();
    }

    public static async void AWait(this Task task, Action? completedCallback, Action<Exception>? errorCallback)
    {
        try
        {
            await task;
            completedCallback?.Invoke();
        }
        catch (Exception exception)
        {
            errorCallback?.Invoke(exception);
        }
    }

    public static bool IsURL(this string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return false;
        }

        if (!url.StartsWith("http"))
        {
            return false;
        }

        if (!url.Contains("://"))
        {
            return false;
        }

        return Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    public static bool IsJsonValid(this string jsonSource)
    {
        JsonDocument? jDoc = null;

        try
        {
            Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(jsonSource));
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