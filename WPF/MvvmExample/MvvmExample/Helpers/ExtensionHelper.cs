using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MvvmExample.Helpers;

public static class ExtensionHelper
{
    public static string RemoveNewline(this string source)
    {
        return string.IsNullOrWhiteSpace(source) ? "" : source.Replace("\n", " ").Replace("\r", " ");
    }

    public static string RemoveDoubleSpace(this string source)
    {
        return string.IsNullOrWhiteSpace(source) ? "" : Regex.Replace(source, @"\s+", $"{(char)32}");
    }

    public static string ReplacePlainText(this string source)
    {
        return source.RemoveNewline().RemoveDoubleSpace();
    }

    public static async void AWait(this Task task, Action completedCallback, Action<Exception> errorCallback)
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
}
