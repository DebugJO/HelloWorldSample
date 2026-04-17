using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using IconPacks.Avalonia.Codicons;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace MyApp.WindowHelper.ThemeHelper;

public class AttachedHelper
{
}

public static class ButtonBusy
{
    public static readonly AttachedProperty<string?> BusyTextProperty =
        AvaloniaProperty.RegisterAttached<Button, string?>("BusyText", typeof(ButtonBusy));

    public static void SetBusyText(Button element, string? value) => element.SetValue(BusyTextProperty, value);

    public static string? GetBusyText(Button element) => element.GetValue(BusyTextProperty);

    private static readonly AttachedProperty<object?> OriginalContentProperty =
        AvaloniaProperty.RegisterAttached<Button, object?>("OriginalContent", typeof(ButtonBusy));

    static ButtonBusy()
    {
        InputElement.IsEnabledProperty.Changed.AddClassHandler<Button>((x, e) =>
        {
            string? busyText = GetBusyText(x);

            if (string.IsNullOrEmpty(busyText))
            {
                return;
            }

            if (!(bool)e.NewValue!)
            {
                x.SetValue(OriginalContentProperty, x.Content);
                x.Content = busyText;
            }
            else
            {
                x.Content = x.GetValue(OriginalContentProperty);
            }
        });
    }
}

public static class AttachedButton
{
    public static readonly AttachedProperty<PackIconCodiconsKind> IconProperty =
        AvaloniaProperty.RegisterAttached<Button, PackIconCodiconsKind>(
            "Icon",
            typeof(AttachedButton));

    public static void SetIcon(Button element, PackIconCodiconsKind value) =>
        element.SetValue(IconProperty, value);

    public static PackIconCodiconsKind GetIcon(Button element) =>
        element.GetValue(IconProperty);

    public static readonly AttachedProperty<double> IconSizeProperty =
        AvaloniaProperty.RegisterAttached<Button, double>(
            "IconSize",
            typeof(AttachedButton),
            16.0);

    public static void SetIconSize(Button element, double value) =>
        element.SetValue(IconSizeProperty, value);

    public static double GetIconSize(Button element) =>
        element.GetValue(IconSizeProperty);
}

public enum TextBoxInputMode
{
    None,
    NumberOnly,
    AlphaNumeric,
    Date,
    PhoneNumber,
    IpAddress
}

public static partial class TextBoxInput
{
    // // class TextBoxInput : AvaloniaObject 사용할 때
    // public static readonly AttachedProperty<TextBoxInputMode> InputModeProperty =
    //     AvaloniaProperty.RegisterAttached<TextBoxInput, TextBox, TextBoxInputMode>("InputMode");

    public static readonly AttachedProperty<TextBoxInputMode> InputModeProperty =
        AvaloniaProperty.RegisterAttached<TextBox, TextBoxInputMode>("Mode", typeof(TextBoxInput));

    public static void SetMode(TextBox element, TextBoxInputMode value) => element.SetValue(InputModeProperty, value);

    public static TextBoxInputMode GetMode(TextBox element) => element.GetValue(InputModeProperty);

    static TextBoxInput()
    {
        InputModeProperty.Changed.AddClassHandler<TextBox>(OnInputModeChanged);
    }

    private static void OnInputModeChanged(TextBox textBox, AvaloniaPropertyChangedEventArgs e)
    {
        textBox.TextInput -= OnTextInput;
        textBox.TextChanged -= OnTextChanged;
        textBox.LostFocus -= OnLostFocus;

        if (e.NewValue is TextBoxInputMode mode && mode != TextBoxInputMode.None)
        {
            textBox.TextInput += OnTextInput;
            textBox.TextChanged += OnTextChanged;
            textBox.LostFocus += OnLostFocus;
            InputMethod.SetIsInputMethodEnabled(textBox, false);
        }
        else
        {
            InputMethod.SetIsInputMethodEnabled(textBox, true);
        }
    }

    private static void OnLostFocus(object? sender, RoutedEventArgs e)
    {
        if (sender is not TextBox textBox)
        {
            return;
        }

        TextBoxInputMode mode = GetMode(textBox);
        string text = textBox.Text ?? string.Empty;

        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        bool isValid = mode switch
        {
            TextBoxInputMode.Date => DateFullRegex().IsMatch(text),
            TextBoxInputMode.PhoneNumber => PhoneFullRegex().IsMatch(text),
            TextBoxInputMode.IpAddress => IpFullRegex().IsMatch(text),
            _ => true
        };

        if (!isValid) textBox.Text = string.Empty;
    }

    private static void OnTextInput(object? sender, TextInputEventArgs e)
    {
        if (sender is not TextBox textBox)
        {
            return;
        }

        TextBoxInputMode mode = GetMode(textBox);
        string input = e.Text ?? string.Empty;

        if (string.IsNullOrEmpty(input))
        {
            return;
        }

        bool isValid = mode switch
        {
            TextBoxInputMode.NumberOnly => NumbersRegex().IsMatch(input),
            TextBoxInputMode.AlphaNumeric => AlphaNumericRegex().IsMatch(input),
            TextBoxInputMode.Date => ValidateDateInput(textBox, input),
            TextBoxInputMode.PhoneNumber => ValidatePhoneInput(textBox, input),
            TextBoxInputMode.IpAddress => ValidateIpInput(textBox, input),
            _ => true
        };

        if (!isValid) e.Handled = true;
    }

    private static bool ValidateDateInput(TextBox textBox, string input)
    {
        string text = textBox.Text ?? string.Empty;
        int pos = textBox.CaretIndex;

        if (input == "-")
        {
            return pos is 4 or 7;
        }

        if (!char.IsDigit(input[0]))
        {
            return false;
        }

        if (pos is 4 or 7 or >= 10)
        {
            return false;
        }

        int lastSep = text.LastIndexOf('-', Math.Max(0, pos - 1));
        int segmentLen = pos - (lastSep + 1);
        int hyphenCount = text.Take(pos).Count(c => c == '-');

        string predicted = (segmentLen == 0)
            ? input
            : string.Concat(text.AsSpan(lastSep + 1, segmentLen), input);

        if (!int.TryParse(predicted, out int val))
        {
            return false;
        }

        switch (hyphenCount)
        {
            case 0 when segmentLen >= 4:
            case 0 when predicted.Length == 4 && val is < 1900 or > 2100:
            case 1 when segmentLen >= 2:
            case 1 when predicted.Length == 1 && val > 1:
            case 1 when predicted.Length == 2 && val is < 1 or > 12:
            case 2 when segmentLen >= 2:
            case 2 when predicted.Length == 1 && val > 3:
            case 2 when predicted.Length == 2 && val is < 1 or > 31:
                return false;
            default:
                return true;
        }
    }

    private static bool ValidatePhoneInput(TextBox textBox, string input)
    {
        string text = textBox.Text ?? string.Empty;
        int pos = textBox.CaretIndex;

        if (input == "-")
        {
            int lastSep = text.LastIndexOf('-', Math.Max(0, pos - 1));
            int segmentLen = pos - (lastSep + 1);
            return pos > 0 && text[pos - 1] != '-' && segmentLen >= 3 && text.Count(c => c == '-') < 2;
        }

        if (!char.IsDigit(input[0]))
        {
            return false;
        }

        {
            int lastSep = text.LastIndexOf('-', Math.Max(0, pos - 1));
            int segmentLen = pos - (lastSep + 1);

            if (segmentLen >= 4)
            {
                return false;
            }

            return pos < 14;
        }
    }

    private static bool ValidateIpInput(TextBox textBox, string input)
    {
        string text = textBox.Text ?? string.Empty;
        int pos = textBox.CaretIndex;

        if (pos >= 15)
        {
            return false;
        }

        int lastSeparator = text.LastIndexOf('.', Math.Max(0, pos - 1));
        int segmentLen = pos - (lastSeparator + 1);

        if (input == ".")
        {
            return text.Count(c => c == '.') < 3 && pos > 0 && text[pos - 1] != '.' && segmentLen >= 1;
        }

        if (!char.IsDigit(input[0]))
        {
            return false;
        }

        if (segmentLen >= 3) return false;
        return int.TryParse(string.Concat(text.AsSpan(lastSeparator + 1, segmentLen), input), out int val) && val <= 255;
    }

    private static void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is not TextBox textBox)
        {
            return;
        }

        string originalText = textBox.Text ?? string.Empty;

        if (string.IsNullOrEmpty(originalText))
        {
            return;
        }

        TextBoxInputMode mode = GetMode(textBox);

        string filteredText = mode switch
        {
            TextBoxInputMode.NumberOnly => LeaveOnlyNumbers(originalText),
            TextBoxInputMode.AlphaNumeric => LeaveOnlyNumberAlpha(originalText),
            TextBoxInputMode.Date => ProcessDateInput(originalText),
            TextBoxInputMode.PhoneNumber => ProcessPhoneInput(originalText),
            TextBoxInputMode.IpAddress => ProcessIpInput(originalText),
            _ => originalText
        };

        if (originalText == filteredText)
        {
            return;
        }

        int caretIndex = textBox.CaretIndex;
        textBox.Text = filteredText;
        textBox.CaretIndex = Math.Min(caretIndex, filteredText.Length);
    }

    private static string LeaveOnlyNumberAlpha(string inString) =>
        new(inString.Where(c => AlphaNumericRegex().IsMatch(c.ToString())).ToArray());

    private static string LeaveOnlyNumbers(string inString) =>
        new(inString.Where(c => NumbersRegex().IsMatch(c.ToString())).ToArray());

    private static string ProcessDateInput(string inString)
    {
        string filtered = new(inString.Where(c => char.IsDigit(c) || c == '-').Take(10).ToArray());
        string[] segments = filtered.Split('-');
        bool changed = false;

        for (int i = 0; i < segments.Length; i++)
        {
            int maxLen = (i == 0) ? 4 : 2;

            if (segments[i].Length > maxLen)
            {
                segments[i] = segments[i].Substring(0, maxLen);
                changed = true;
            }

            if (!int.TryParse(segments[i], out int val))
            {
                continue;
            }

            switch (i)
            {
                case 0 when segments[i].Length == 4:
                {
                    switch (val)
                    {
                        case < 1900:
                            segments[i] = "1900";
                            changed = true;
                            break;
                        case > 2100:
                            segments[i] = "2100";
                            changed = true;
                            break;
                    }

                    break;
                }

                case 1 when segments[i].Length == 2:
                {
                    switch (val)
                    {
                        case > 12:
                            segments[i] = "12";
                            changed = true;
                            break;
                        case < 1 when segments[i] == "00":
                            segments[i] = "01";
                            changed = true;
                            break;
                    }

                    break;
                }

                case 2 when segments[i].Length == 2:
                {
                    switch (val)
                    {
                        case > 31:
                            segments[i] = "31";
                            changed = true;
                            break;
                        case < 1 when segments[i] == "00":
                            segments[i] = "01";
                            changed = true;
                            break;
                    }

                    break;
                }
            }
        }

        return changed ? string.Join("-", segments) : filtered;
    }

    private static string ProcessPhoneInput(string inString)
    {
        string filtered = new(inString.Where(c => char.IsDigit(c) || c == '-').Take(14).ToArray());
        string[] segments = filtered.Split('-');

        for (int i = 0; i < segments.Length; i++)
        {
            if (segments[i].Length > 4) segments[i] = segments[i][..4];
        }

        return string.Join("-", segments);
    }

    private static string ProcessIpInput(string inString)
    {
        string filtered = new(inString.Where(c => char.IsDigit(c) || c == '.').Take(15).ToArray());
        string[] parts = filtered.Split('.');
        bool changed = false;

        for (int i = 0; i < parts.Length; i++)
        {
            if (i >= 4 || !int.TryParse(parts[i], out int val) || val <= 255)
            {
                continue;
            }

            parts[i] = "255";
            changed = true;
        }

        if (parts.Length > 4)
        {
            return string.Join(".", parts.Take(4));
        }

        return changed ? string.Join(".", parts) : filtered;
    }

    [GeneratedRegex("^[0-9a-zA-Z]*$")]
    private static partial Regex AlphaNumericRegex();

    [GeneratedRegex("^[0-9]*$")]
    private static partial Regex NumbersRegex();

    [GeneratedRegex(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$")]
    private static partial Regex DateFullRegex();

    [GeneratedRegex(@"^(\d{4}-\d{4}-\d{4}|\d{3}-\d{4}-\d{4}|\d{3}-\d{3}-\d{4})$")]
    private static partial Regex PhoneFullRegex();

    [GeneratedRegex(@"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$")]
    private static partial Regex IpFullRegex();
}