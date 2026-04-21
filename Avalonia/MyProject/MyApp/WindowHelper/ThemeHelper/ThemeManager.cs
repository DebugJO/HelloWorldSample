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

    // public static string GetName(ThemeVariant variant)
    // {
    //     return variant switch
    //     {
    //         var v when v == ThemeVariant.Light => Light,
    //         var v when v == ThemeVariant.Dark => Dark,
    //         var v when v == CustomThemes.DeepBlue => DeepBlue,
    //         var v when v == CustomThemes.MsWordLight => MsWordLight,
    //         var v when v == CustomThemes.MsWordDark => MsWordDark,
    //         _ => Light
    //     };
    // }
    
    // 테마적용
    // app.RequestedThemeVariant = ThemeManager.GetVariant(mainState.Theme);
    // 테마저장
    // var currentVariant = Application.Current.RequestedThemeVariant;
    // string themeName = ThemeManager.GetName(currentVariant);
}