using Avalonia;
using Avalonia.Styling;

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

    public static string GetTheme(ETheme theme)
    {
        return theme switch
        {
            ETheme.Light => GetName(ThemeVariant.Light),
            ETheme.Dark => GetName(ThemeVariant.Dark),
            ETheme.DeepBlue => GetName(CustomThemes.DeepBlue),
            ETheme.MsWordLight => GetName(CustomThemes.MsWordLight),
            ETheme.MsWordDark => GetName(CustomThemes.MsWordDark),
            _ => GetName(ThemeVariant.Default)
        };
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
    private static string GetName(ThemeVariant variant)
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
}

public enum ETheme
{
    Light,
    Dark,
    DeepBlue,
    MsWordLight,
    MsWordDark
}