using Avalonia.Controls;
using MyApp.WindowHelper;
using MyAppLib.Helpers;
using System;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyApp.StateModels;

public class MainState : IJsonConvertible
{
    [JsonIgnore]
    public string StatusMessage { get; set; } = string.Empty;

    public string Theme { get; set; } = "Light";

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public WindowState WindowState { get; set; } = WindowState.Normal;

    public string LastUserId { get; set; } = string.Empty;
    public string LastUserPassword { get; set; } = string.Empty;

    // [JsonIgnore]
    // public ObservableCollection<string> Items { get;  } = [];

    [JsonIgnore]
    private readonly string FilePath = PathConfig.ConfigFilePath;

    public async Task<Result<MainState>> LoadAsync()
    {
        try
        {
#if DEBUG
            LogHelper.Debug($"AppConfig : Load, Config 파일 Path : {FilePath}");
#endif
            if (!File.Exists(FilePath))
            {
                return None.Value;
            }

            string json = await File.ReadAllTextAsync(FilePath);
            MainState mainState = json.FromJson<MainState>();
            return mainState;
        }
        catch (Exception ex)
        {
            string error = $"AppConfig : Failed to load config file : {ex.Message}";
            return Result<MainState>.Failure(error);
        }
    }

    public async Task<Result<bool>> SaveAsync(MainState appConfig)
    {
        try
        {
#if DEBUG
            LogHelper.Debug($"AppConfig : Save, Config 파일 Path : {FilePath}");
#endif
            string json = appConfig.ToJson(true);

            if (string.IsNullOrWhiteSpace(json))
            {
                return None.Value;
            }

            await File.WriteAllTextAsync(FilePath, json);
            return true;
        }
        catch (Exception ex)
        {
            string error = $"AppConfig : Failed to save config file : {ex.Message}";
            return Result<bool>.Failure(error);
        }
    }
}