using Avalonia.Logging;
using MyAppLib.Helpers;

namespace MyApp.WindowHelper;

public class CustomLogSink(LogEventLevel minimumLevel, string title = "AVALONIA") : ILogSink
{
    public bool IsEnabled(LogEventLevel level, string area)
    {
        return level >= minimumLevel || area == LogArea.Binding;
        // return level >= LogEventLevel.Error || area == LogArea.Binding;
    }

    public void Log(LogEventLevel level, string area, object? source, string messageTemplate)
    {
        ProcessLog(level, area, source, messageTemplate, null);
    }

    public void Log(LogEventLevel level, string area, object? source, string messageTemplate, params object?[]? propertyValues)
    {
        ProcessLog(level, area, source, messageTemplate, propertyValues);
    }

    private void ProcessLog(LogEventLevel level, string area, object? source, string messageTemplate, object?[]? propertyValues)
    {
        if (level < LogEventLevel.Error && area != LogArea.Binding)
        {
            return;
        }

        string message = messageTemplate;

        try
        {
            if (propertyValues is { Length: > 0 })
            {
                message = string.Format(messageTemplate, propertyValues);
            }
        }
        catch
        {
            if (propertyValues != null && propertyValues.Length > 0)
            {
                message = $"{messageTemplate} | Args: {string.Join(", ", propertyValues)}";
            }
        }

        string sourceInfo = source != null ? $" [Source: {source.GetType().Name}]" : "";
        string header = (area == LogArea.Binding) ? "[BINDING ERROR]" : $"[{title}][{level}][{area}]";
        string fullLog = $"{header}{sourceInfo} {message}";
        // Debug.WriteLine(fullLog);
        LogHelper.Fatal($"전역에러(Avalonia) : {fullLog}");
    }
}