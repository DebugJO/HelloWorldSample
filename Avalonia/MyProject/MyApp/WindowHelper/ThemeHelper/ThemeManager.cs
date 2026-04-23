using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using MyApp.Views;
using MyAppLib.Helpers;
using System;

namespace MyApp.WindowHelper.ThemeHelper;

public static class ThemeManager
{
    public const string Default = "Default";
    public const string Light = "Light";
    public const string Dark = "Dark";
    public const string DeepBlue = "DeepBlue";
    public const string MsWordLight = "MsWordLight";
    public const string MsWordDark = "MsWordDark";

    public static void ApplyTheme(string theme)
    {
        Application? app = Application.Current;
        app?.RequestedThemeVariant = GetVariant(theme);
    }

    // app.RequestedThemeVariant = ThemeManager.GetVariant(mainState.Theme);
    public static ThemeVariant GetVariant(string name)
    {
        return name switch
        {
            Light => ThemeVariant.Light,
            Dark => ThemeVariant.Dark,
            DeepBlue => CustomThemes.DeepBlue,
            MsWordLight => CustomThemes.MsWordLight,
            MsWordDark => CustomThemes.MsWordDark,
            _ => ThemeVariant.Default
        };
    }

    // var currentVariant = Application.Current.RequestedThemeVariant;
    // string themeName = ThemeManager.GetName(currentVariant);
    public static string GetName(ThemeVariant variant)
    {
        return variant switch
        {
            _ when variant == ThemeVariant.Light => Light,
            _ when variant == ThemeVariant.Dark => Dark,
            _ when variant == CustomThemes.DeepBlue => DeepBlue,
            _ when variant == CustomThemes.MsWordLight => MsWordLight,
            _ when variant == CustomThemes.MsWordDark => MsWordDark,
            _ => Default
        };
    }

    // IBrush? themeBrush = ThemeManager.GetThemeBrush(Theme, "DangerBrush");
    // mainView.Background = themeBrush;
    public static IBrush? GetThemeBrush(string themeName, string resourceName, bool systemRegionBrush = false)
    {
        try
        {
            if (systemRegionBrush)
            {
                return themeName switch
                {
                    Light => Brush.Parse("#F3F2F1"),
                    Dark => Brush.Parse("#21252B"),
                    DeepBlue => Brush.Parse("#0A192F"),
                    MsWordLight => Brush.Parse("#F3F2F1"),
                    MsWordDark => Brush.Parse("#2B579A"),
                    _ => Brushes.Transparent
                };
            }

            MainView mainView = DI.Get<MainView>();
            ThemeVariant themeVariant = GetVariant(themeName);

            if (mainView.TryFindResource(resourceName, themeVariant, out object? res)
                && res is IBrush themeBrush)
            {
                return themeBrush;
            }

            return null;
        }
        catch (Exception ex)
        {
            LogHelper.Error($"Theme Manager : x : {ex.Message}");
            return null;
        }
    }

    // IBrush themeBrush = ThemeManager.GetSystemBrush(Theme);
    // mainView.Background = themeBrush;
    public static IBrush GetSystemBrush(string themeName)
    {
        return themeName switch
        {
            Light => Brush.Parse("#F3F2F1"),
            Dark => Brush.Parse("#21252B"),
            DeepBlue => Brush.Parse("#0A192F"),
            MsWordLight => Brush.Parse("#F3F2F1"),
            MsWordDark => Brush.Parse("#2B579A"),
            _ => Brush.Parse("#0078D4"),
        };
    }
}

// public static string GetTheme(ETheme theme)
// {
//     return theme switch
//     {
//         ETheme.Light => GetName(ThemeVariant.Light),
//         ETheme.Dark => GetName(ThemeVariant.Dark),
//         ETheme.DeepBlue => GetName(CustomThemes.DeepBlue),
//         ETheme.MsWordLight => GetName(CustomThemes.MsWordLight),
//         ETheme.MsWordDark => GetName(CustomThemes.MsWordDark),
//         _ => GetName(ThemeVariant.Default)
//     };
// }

// public enum ETheme
// {
//     Light,
//     Dark,
//     DeepBlue,
//     MsWordLight,
//     MsWordDark
// }