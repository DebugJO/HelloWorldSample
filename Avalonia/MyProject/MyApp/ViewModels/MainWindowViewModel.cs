using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.VisualTree;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyApp.Controls;
using MyAppLib.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private bool _isInitialized;
    private bool _isForceClose;
    private bool _isBusy;

    [ObservableProperty]
    public partial string StatusMessage { get; set; } = string.Empty;

    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task AppStartAsync()
    {
        if (_isInitialized)
        {
            return;
        }

        try
        {
            await Task.WhenAny(AppStart(), Task.Delay(3000));
        }
        catch (Exception ex)
        {
            LogHelper.Error($"AppStart : 초기화 오류 : {ex.Message}");
            StatusMessage = "AppStart : 초기화 오류 발생";
        }
        finally
        {
            _isInitialized = true;
        }
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task AppClosingAsync(CancelEventArgs e)
    {
        if (_isForceClose)
        {
            return;
        }

        e.Cancel = true;

        if (_isBusy)
        {
            return;
        }

        _isBusy = true;

        try
        {
            await Task.WhenAny(AppStop(), Task.Delay(3000));
        }
        catch (Exception ex)
        {
            LogHelper.Error("Closing Error: " + ex.Message);
        }
        finally
        {
            _isForceClose = true;
            _isBusy = false;
            RequestClose();
        }
    }

    private void RequestClose()
    {
        LogHelper.Debug("Desktop : RequestClose : 윈도우 종료 요청 ...");

        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
        {
            return;
        }

        List<Window> subWindows = desktop.Windows.Skip(1).ToList();

        foreach (Window window in subWindows)
        {
            window.Close();
        }

        desktop.MainWindow?.Close();
    }

    private async Task AppStart()
    {
        LogHelper.Debug("AppStart : 자원 할당 시작 ...");

        try
        {
            StatusMessage = "데이터베이스 연결 중 ...";
            await Task.Delay(500);
            StatusMessage = "데이터베이스 연결 완료";
            LogHelper.Debug("AppStart : 데이터베이스 연결 완료");

            StatusMessage = "MQTT 브로커 접속 중...";
            await Task.Delay(500);
            StatusMessage = " MQTT 브로커 접속 완료";
            LogHelper.Debug("AppStart : MQTT 브로커 접속 완료");

            StatusMessage = "서비스 시작 완료";
            LogHelper.Debug("AppStart : 서비스 시작 완료");
        }
        catch (Exception ex)
        {
            LogHelper.Error($"AppStart : 자원 할당 시작 중 오류 : {ex.Message}");
            StatusMessage = "AppStart : 자원 할당 시작 중 오류 발생";
        }

        LogHelper.Debug("AppStart : 자원 설정 완료 ...");
    }

    private async Task AppStop()
    {
        LogHelper.Debug("AppStop : 자원 해재 시작 ...");

        try
        {
            StatusMessage = "데이터베이스 해제 중 ...";
            await Task.Delay(500);
            StatusMessage = "데이터베이스 해제 완료";
            LogHelper.Debug("AppStop : 데이터베이스 해제 완료");

            StatusMessage = "MQTT 브로커 해제 중...";
            await Task.Delay(500);
            StatusMessage = " MQTT 브로커 해제 완료";
            LogHelper.Debug("AppStop : MQTT 브로커 해제 완료");

            StatusMessage = "서비스 해제 완료";
            LogHelper.Debug("AppStop : 서비스 해제 완료");
        }
        catch (Exception ex)
        {
            LogHelper.Error($"AppStop : 자원 해제 중 오류 : {ex.Message}");
        }

        LogHelper.Debug("AppStop : 자원 해제 완료 ...");
    }

    [RelayCommand]
    private void TitleBarAction(PointerPressedEventArgs e)
    {
        if (e.Source is not Visual visual || visual.GetVisualRoot() is not Window window)
        {
            return;
        }

        PointerPointProperties pointerProps = e.GetCurrentPoint(window).Properties;

        if (!pointerProps.IsLeftButtonPressed)
        {
            return;
        }

        if (e.ClickCount == 2)
        {
            window.WindowState = window.WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }
        else
        {
            window.BeginMoveDrag(e);
        }
    }

    [RelayCommand]
    private void ToggleWindowState(Window? window)
    {
        window?.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    [RelayCommand]
    private void MinimizeWindow(Window? window)
    {
        window?.WindowState = WindowState.Minimized;
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task CloseWindowAsync(Window? window)
    {
        if (window == null)
        {
            return;
        }

        bool result = await ShowConfirmDialog("종료하시겠습니까?");

        if (result)
        {
            window.Close();
        }
    }

    private static async Task<bool> ShowConfirmDialog(string message)
    {
        LogHelper.Debug(message);
        await Task.Delay(1);
        return true;
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task ThemeChangeAsync(Button? button)
    {
        if (button == null)
        {
            return;
        }

        
        /*
         
         메인윈도우 가운데 정렬
         
         
         */
        
        // Application app = Application.Current!;
        // app.RequestedThemeVariant = Themes.DeepBlue;

        MsgBoxResult result = await CustomMessageBox.ShowAsync(
            "정말 삭제하시겠습니까?",
            "삭제 확인",
            MsgBoxButtons.YesNo,
            MsgBoxIcon.Question);
        
        LogHelper.Debug(result == MsgBoxResult.Yes ? "yes" : "no");

        await Task.CompletedTask;
    }
}

//
// try
// {
//     Application app = Application.Current!;
//     ThemeVariant? current = app.RequestedThemeVariant;
//
//     if (current == ThemeVariant.Light)
//         app.RequestedThemeVariant = ThemeVariant.Dark;
//     else if (current == ThemeVariant.Dark)
//         app.RequestedThemeVariant = Themes.DeepBlue;
//     else if (current == Themes.DeepBlue)
//         app.RequestedThemeVariant = Themes.MsWordLight;
//     else if (current == Themes.MsWordLight)
//         app.RequestedThemeVariant = Themes.MsWordDark;
//     else
//         app.RequestedThemeVariant = ThemeVariant.Light;
// }
// catch (Exception ex)
// {
//     LogHelper.Error("ThemeChange Error: " + ex.Message);
// }
// await Task.CompletedTask;
//
// }
// }