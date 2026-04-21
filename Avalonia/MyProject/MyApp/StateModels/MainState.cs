using Avalonia.Controls;
using Avalonia.Styling;
using MyAppLib.Helpers;
using System.Text.Json.Serialization;

namespace MyApp.StateModels;

public class MainState : IJsonConvertible
{
    public string StatusMessage { get; set; } = string.Empty;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public WindowState WindowState { get; set; } = WindowState.Normal;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public  ThemeVariant Theme { get; set; } = ThemeVariant.Dark;
    public double FontSize { get; set; } = 12.0;
    public string LastUserId { get; set; } = string.Empty;
    public string LastUserPassword { get; set; } = string.Empty;

    // public ObservableCollection<string> Items { get;  } = [];
}