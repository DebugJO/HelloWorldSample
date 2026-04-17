using Avalonia.Data.Converters;
using IconPacks.Avalonia.Codicons;
using System;
using System.Globalization;

namespace MyApp.WindowHelper.ThemeHelper;

public class ConverterHelper
{
}

public class IconToVisibleConverter : IValueConverter
{
    public static readonly IconToVisibleConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is PackIconCodiconsKind kind)
        {
            return kind != PackIconCodiconsKind.None;
        }

        return false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // throw new NotImplementedException();
        return Avalonia.AvaloniaProperty.UnsetValue;
    }
}

public class RemoveLineBreakConverter : IValueConverter
{
    private readonly string space = ((char)32).ToString();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string text)
        {
            return text.Replace("\r\n", space).Replace("\n", space);
        }

        return value;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Avalonia.AvaloniaProperty.UnsetValue;
    }
}