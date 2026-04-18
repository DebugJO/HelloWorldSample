using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace MyApp.WindowHelper.ThemeHelper;

public class ConverterHelper
{
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