﻿using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace WebApiTest.Helpers;

public static class LogHelper
{
    public static readonly Logger Logger;

    static LogHelper()
    {
        LoggingConfiguration config = new();
        FileTarget target = new()
        {
            FileName = "${basedir}/AppLogs/${date:format=yyyyMM}/log_${date:format=yyyyMMdd}.txt",
            Layout = @"[${date:format=HH\:mm\:ss} ${level:uppercase=true:padding=5} : PID ${processid:Padding=5}] ${message} (${callsite:className=true:includeNamespace=false:fileName=false:includeSourcePath=false:methodName=true}[${callsite-linenumber}][${threadid}])",
            ArchiveEvery = FileArchivePeriod.Month
        };

        AsyncTargetWrapper asyncWrapper = new(target, 1000, AsyncTargetWrapperOverflowAction.Grow);
        LoggingRule rule = new("*", LogLevel.Trace, asyncWrapper);
        config.LoggingRules.Add(rule);

#if DEBUG
        OutputDebugStringTarget targetOutput = new()
        {
            Layout = target.Layout
        };

        AsyncTargetWrapper asyncWrapperOutput = new(targetOutput, 1000, AsyncTargetWrapperOverflowAction.Grow);
        LoggingRule ruleOutput = new("*", LogLevel.Trace, asyncWrapperOutput);
        config.LoggingRules.Add(ruleOutput);
#endif

        LogManager.Configuration = config;
        Logger = LogManager.GetLogger("LogHelper");
    }

    public static void ShutdownLogManager()
    {
        LogManager.Flush();
        LogManager.Shutdown();
    }
}