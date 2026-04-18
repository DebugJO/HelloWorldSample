using MyAppLib.AppStates;
using MyAppLib.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MyAppLib.Services;

public class ConfigService : IConfigService
{
    private readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "config.json");

    
    public async Task<AppConfigState> Load()
    {
        if (!File.Exists(FilePath))
        {
            LogHelper.Warn("Config file not found");
            return new AppConfigState();
        }

        try
        {
            string json = await File.ReadAllTextAsync(FilePath);
            return json.FromJson<AppConfigState>();
        }
        catch (Exception ex)
        {
            LogHelper.Error($"Failed to load config file: {ex.Message}");
            return new AppConfigState();
        }
    }

    public async Task Save(AppConfigState appConfig)
    {
        try
        {
            string json = appConfig.ToJson(true);
            await File.WriteAllTextAsync(FilePath, json);
        }
        catch (Exception ex)
        {
            LogHelper.Error($"Failed to save config file: {ex.Message}");
        }
    }
}