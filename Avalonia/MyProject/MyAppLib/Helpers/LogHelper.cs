using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System;
using System.Runtime.CompilerServices;

namespace MyAppLib.Helpers;

public class LogHelper
{
    private static readonly Logger _logger;
    private const string EX = " >> ";

    static LogHelper()
    {
        LoggingConfiguration config = new();

        const string common_layout =
            @"[${date:format=HH\:mm\:ss} ${level:uppercase=true:padding=5} : PID ${processid:Padding=5}] ${message}${onexception:${exception:format=message}} (${callsite:className=true:includeNamespace=false:methodName=true}[${callsite-linenumber}][${threadid}])";

        const string log_folder = "${basedir}/AppLogs/${date:format=yyyyMM}";

        FileTarget fileTarget = new("FileTarget")
        {
            FileName = log_folder + "/${date:format=yyyyMMdd}_log.txt",
            ArchiveFileName = log_folder + "/${date:format=yyyyMMdd}_log.{#}.txt",
            Layout = common_layout,
            Encoding = System.Text.Encoding.UTF8,
            KeepFileOpen = true,
            OpenFileCacheTimeout = 30,
            AutoFlush = false,
            BufferSize = 65536,
            ArchiveEvery = FileArchivePeriod.Day,
            MaxArchiveDays = 90,
            ArchiveAboveSize = 10485760
        };

        AsyncTargetWrapper asyncWrapper = new(fileTarget, 10000, AsyncTargetWrapperOverflowAction.Grow)
        {
            TimeToSleepBetweenBatches = 0
        };

        config.AddRule(LogLevel.Trace, LogLevel.Fatal, asyncWrapper);

#if DEBUG
        OutputDebugStringTarget debugOutput = new("debugOutput")
        {
            Layout = common_layout
        };
        AsyncTargetWrapper asyncDebug = new(debugOutput, 5000, AsyncTargetWrapperOverflowAction.Discard);
        config.AddRule(LogLevel.Trace, LogLevel.Fatal, asyncDebug);
#endif
        LogManager.Configuration = config;
        _logger = LogManager.GetCurrentClassLogger();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void WriteLog(LogLevel level, string message, Exception? ex = null)
    {
        if (!_logger.IsEnabled(level))
        {
            return;
        }

        try
        {
            LogEventInfo logEvent = new(level, "LogHelper", message)
            {
                Exception = ex
            };

            _logger.Log(typeof(LogHelper), logEvent);
        }
        catch
        {
            // ignored
        }
    }

    public static void Trace(string msg) => WriteLog(LogLevel.Trace, msg);

    public static void Debug(string msg) => WriteLog(LogLevel.Debug, msg);

    public static void Info(string msg) => WriteLog(LogLevel.Info, msg);

    public static void Warn(string msg) => WriteLog(LogLevel.Warn, msg);

    public static void Error(string msg) => WriteLog(LogLevel.Error, msg);

    public static void Error(Exception ex, string msg) => WriteLog(LogLevel.Error, msg + EX, ex);

    public static void Fatal(string msg) => WriteLog(LogLevel.Fatal, msg);

    public static void Fatal(Exception ex, string msg) => WriteLog(LogLevel.Fatal, msg + EX, ex);

    public static void Shutdown()
    {
        try
        {
            LogManager.Flush(TimeSpan.FromSeconds(2));
            LogManager.Shutdown();
        }
        catch
        {
            // ignored
        }
    }
}