using Avalonia.Styling;

namespace MyApp.WindowHelper.ThemeHelper;

public static class CustomThemes
{
    public static readonly ThemeVariant DeepBlue = new("DeepBlue", ThemeVariant.Dark);
    public static readonly ThemeVariant MsWordLight = new("MsWordLight", ThemeVariant.Light);
    public static readonly ThemeVariant MsWordDark = new("MsWordDark", ThemeVariant.Dark);
}