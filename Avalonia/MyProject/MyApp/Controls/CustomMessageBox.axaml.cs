using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml.MarkupExtensions;
using IconPacks.Avalonia.Codicons;
using System.Threading.Tasks;

namespace MyApp.Controls;

public partial class MessageBox : Window
{
    public MsgBoxResult Result { get; set; } = MsgBoxResult.Cancel;

    public MessageBox()
    {
        InitializeComponent();
    }

    public static async Task<MsgBoxResult> ShowAsync(string message, string caption, MsgBoxButtons buttons, MsgBoxIcon icon = MsgBoxIcon.None)
    {
        MessageBox dialog = CreateDialog(message, caption, buttons, icon);
        IClassicDesktopStyleApplicationLifetime? lifetime = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
        Window? mainWindow = lifetime?.MainWindow;

        if (mainWindow == null)
        {
            return MsgBoxResult.Cancel;
        }

        MsgBoxResult? result = await dialog.ShowDialog<MsgBoxResult?>(mainWindow);
        return result ?? MsgBoxResult.Cancel;
    }

    private static MessageBox CreateDialog(string message, string caption, MsgBoxButtons buttons, MsgBoxIcon icon)
    {
        MessageBox dialog = new()
        {
            CaptionText =
            {
                Text = caption
            },
            MessageText =
            {
                Text = message
            },
            BtnOk =
            {
                IsVisible = buttons == MsgBoxButtons.Ok
            },
            BtnYes =
            {
                IsVisible = buttons is MsgBoxButtons.YesNo or MsgBoxButtons.YesNoCancel
            },
            BtnNo =
            {
                IsVisible = buttons is MsgBoxButtons.YesNo or MsgBoxButtons.YesNoCancel
            },
            BtnCancel =
            {
                IsVisible = buttons == MsgBoxButtons.YesNoCancel
            }
        };

        if (icon == MsgBoxIcon.None)
        {
            dialog.MsgIcon.IsVisible = false;
        }
        else
        {
            dialog.MsgIcon.IsVisible = true;
            (PackIconCodiconsKind codicon, string resourceKey) = icon switch
            {
                MsgBoxIcon.Info => (PackIconCodiconsKind.Info, "AccentBrush"),
                MsgBoxIcon.Warning => (PackIconCodiconsKind.Warning, "DangerBrush"),
                MsgBoxIcon.Error => (PackIconCodiconsKind.Error, "DangerBrush"),
                MsgBoxIcon.Question => (PackIconCodiconsKind.Question, "SuccessBrush"),
                _ => (PackIconCodiconsKind.Info, "SystemControlForegroundBaseHighBrush")
            };

            dialog.MsgIcon.Kind = codicon;
            dialog.MsgIcon[!ForegroundProperty] = new DynamicResourceExtension(resourceKey);
        }

        // 무조건 모니터 중앙
        // WindowCenter.SetCenterOnPrimary(dialog, true);
        return dialog;
    }

    private void OnButtonClick(object sender, RoutedEventArgs e)
    {
        if (sender is not Button btn)
        {
            return;
        }

        MsgBoxResult result = btn.Name switch
        {
            "BtnOk" => MsgBoxResult.Ok,
            "BtnYes" => MsgBoxResult.Yes,
            "BtnNo" => MsgBoxResult.No,
            _ => MsgBoxResult.Cancel
        };

        Close(result);
    }
}

public enum MsgBoxButtons
{
    Ok,
    YesNo,
    YesNoCancel
}

public enum MsgBoxResult
{
    Ok,
    Yes,
    No,
    Cancel
}

public enum MsgBoxIcon
{
    None,
    Info,
    Warning,
    Error,
    Question
}