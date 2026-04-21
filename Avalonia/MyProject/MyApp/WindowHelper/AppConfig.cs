using MyApp.StateModels;
using MyAppLib.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MyApp.WindowHelper;

public static class AppConfig
{
    // private static readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "config.json");
    private static readonly string FilePath = PathConfig.ConfigFilePath;

    public static async Task<MainState> Load()
    {
        try
        {
#if DEBUG
            LogHelper.Debug($"AppConfig : Load, Config 파일 Path : {FilePath}");
#endif
            if (!File.Exists(FilePath))
            {
                LogHelper.Warn("AppConfig : Config file not found or Program initialization");
                return new MainState();
            }

            string json = await File.ReadAllTextAsync(FilePath);
            return json.FromJson<MainState>();
        }
        catch (Exception ex)
        {
            LogHelper.Error($"AppConfig : Failed to load config file: {ex.Message}");
            return new MainState();
        }
    }

    public static async Task Save(MainState appConfig)
    {
        try
        {
#if DEBUG
            LogHelper.Debug($"AppConfig : Save, Config 파일 Path : {FilePath}");
#endif
            string json = appConfig.ToJson(true);
            await File.WriteAllTextAsync(FilePath, json);
        }
        catch (Exception ex)
        {
            LogHelper.Error($"AppConfig : Failed to save config file: {ex.Message}");
        }
    }
}