using System;
using System.IO;

namespace MyApp.WindowHelper;

public static class PathConfig
{
    public static readonly string LockFilePath;
    public static readonly string ConfigFilePath;

    static PathConfig()
    {
        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string appFolderPath = Path.Combine(localAppData, "devsight-kr", "MyApp2026");
        LockFilePath = Path.Combine(appFolderPath, "shutdown.lock");
        ConfigFilePath = Path.Combine(appFolderPath, "config.json");
        
        if (!Directory.Exists(appFolderPath))
        {
            Directory.CreateDirectory(appFolderPath);
        }
    }
}