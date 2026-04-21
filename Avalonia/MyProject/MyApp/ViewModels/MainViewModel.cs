using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Styling;
using Avalonia.VisualTree;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MyApp.Controls;
using MyApp.Messages;
using MyApp.StateModels;
using MyAppLib.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    // Start : 상태 클래스 
    private readonly MainState _mainState;
    // End : 상태 클래스

    // Start : 내부 변수
    private bool _isStarted;
    private bool _isForceClose;
    private bool _isBusy;
    // End : 내부 변수

    // Start : MainState
    [ObservableProperty]
    public partial string StatusMessage { get; set; }

    [ObservableProperty]
    public partial WindowState WindowState { get; set; }

    [ObservableProperty]
    public partial ThemeVariant Theme { get; set; }

    [ObservableProperty]
    public partial double FontSize { get; set; }

    [ObservableProperty]
    public partial string LastUserId { get; set; }

    [ObservableProperty]
    public partial string LastUserPassword { get; set; }
    // public ObservableCollection<string> Items { get; }
    // End : MainState

    // Start : MainViewModel(ContentControl)
    [ObservableProperty]
    // public partial object? CurrentPage { get; set; }
    public partial ViewModelBase? CurrentPage { get; set; }
    // End : MainViewModel(ContentControl)

    public MainViewModel(MainState mainState)
    {
        _mainState = mainState;

        StatusMessage = _mainState.StatusMessage;
        WindowState = mainState.WindowState;
        Theme = _mainState.Theme;
        FontSize = _mainState.FontSize;
        LastUserId = _mainState.LastUserId;
        LastUserPassword = _mainState.LastUserPassword;
        // Items = _state.Items;

        // CurrentPage =  DI.Get<Sub1ViewModel>();
        RegisterStatusMessage();
    }

    private void RegisterStatusMessage()
    {
        WeakReferenceMessenger.Default.Register<StatusMessageRegister>(
            this, (_, m) => { UpdateStatus(m.text); });
    }

    private void UpdateStatus(string value, bool syncState = true)
    {
        if (StatusMessage == value)
        {
            return;
        }

        StatusMessage = value;

        if (syncState)
        {
            _mainState.StatusMessage = value;
        }
#if DEBUG
        LogHelper.Info($"StatusMessage changed: {value}");
#endif
    }

    // private void UpdateConfig()
    // {
    //     _state.Theme = Theme;
    //     _state.FontSize = FontSize;
    //     _state.LastUserId = LastUserId;
    //     _state.LastUserPassword = LastUserPassword;
    // }

    // public void ClickMenu2() => CurrentPage = DI.Get<Sub1ViewModel>();
    // public void ClickMenu2() => CurrentPage = DI.Get<Sub2ViewModel>();
    // StackPanel>
    // <Button Content="서버 1" Command="{Binding ClickMenu1}" />
    // <Button Content="서버 2" Command="{Binding ClickMenu2}" />
    // <Button Content="서버 3" Command="{Binding ClickMenu3}" />
    // </StackPanel>

    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task AppStartAsync()
    {
        if (_isStarted)
        {
            return;
        }

        try
        {
            const int time = 10;
            Task appStartTask = AppStart();
            Task timeoutTask = Task.Delay(TimeSpan.FromSeconds(time));
            Task completedTask = await Task.WhenAny(appStartTask, timeoutTask);

            if (completedTask == timeoutTask)
            {
                LogHelper.Error($"시스템 시작 실패 : AppStart 작업이 {time}초 안에 완료되지 않았습니다.");
                SendState($"시스템 시작 실패 : AppStart 작업이 {time}초 안에 완료되지 않았습니다.");
            }
            else
            {
                await appStartTask;
            }
        }
        catch (Exception ex)
        {
            LogHelper.Error("AppStart Error : " + ex.Message);
        }
        finally
        {
            _isStarted = true;
        }
    }

    private async Task AppStart()
    {
        LogHelper.Debug("AppStart : 자원 할당 시작 ...");

        try
        {
            // AppConfigState configState = await mAppConfigService.Load();
            // Theme = configState.Theme;
            // FontSize = configState.FontSize;
            // LastUserId = configState.LastUserId;
            // LastUserPassword = configState.LastUserPassword;
            //
            // ApplyTheme(configState.Theme);
            // UpdateConfig();
            // LogHelper.Debug("AppService : AppConfig : Load ...");

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

        LogHelper.Debug("AppStart : 자원 설정 완료");
        LogHelper.Debug("========== 프로그램 시작 End ==========");
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
            const int time = 10;
            Task appStopTask = AppStop();
            Task timeoutTask = Task.Delay(TimeSpan.FromSeconds(time));
            Task completedTask = await Task.WhenAny(appStopTask, timeoutTask);

            if (completedTask == timeoutTask)
            {
                LogHelper.Error($"시스템 종료 실패 : AppStop 작업이 {time}초 안에 완료되지 않았습니다.");
                SendState($"시스템 종료 실패 : AppStop 작업이 {time}초 안에 완료되지 않았습니다.");
            }
            else
            {
                await appStopTask;
            }
        }
        catch (Exception ex)
        {
            LogHelper.Error("AppStop Error : " + ex.Message);
        }
        finally
        {
            _isForceClose = true;
            _isBusy = false;
            RequestClose();
        }
    }

    private async Task AppStop()
    {
        LogHelper.Debug("========== 프로그램 종료 Start ==========");
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

        LogHelper.Debug("AppStop : 자원 해제 완료");
    }

    private void RequestClose()
    {
        LogHelper.Debug("AppStop : Request Close : 윈도우 종료 요청");

        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
        {
            return;
        }

        List<Window> subWindows = desktop.Windows
            .Where(w => w != desktop.MainWindow)
            .ToList();

        foreach (Window window in subWindows)
        {
            window.Close();
        }

        desktop.MainWindow?.Close();
    }

    // private void ApplyTheme(string themeName)
    // {
    //     Application app = Application.Current!;
    //     ThemeVariant? current = app.RequestedThemeVariant;
    //
    //     const string light = nameof(ThemeVariant.Light);
    //     const string dark = nameof(ThemeVariant.Dark);
    //     const string deepBlue = nameof(CustomThemes.DeepBlue);
    //     const string msWordLight = nameof(CustomThemes.MsWordLight);
    //     const string msWordDark = nameof(CustomThemes.MsWordDark);
    //
    //     if (current is not null)
    //     {
    //         app.RequestedThemeVariant = themeName switch
    //         {
    //             light => ThemeVariant.Light,
    //             dark => ThemeVariant.Dark,
    //             deepBlue => CustomThemes.DeepBlue,
    //             msWordLight => CustomThemes.MsWordLight,
    //             msWordDark => CustomThemes.MsWordDark,
    //             _ => app.RequestedThemeVariant
    //         };
    //     }
    // }


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

        MsgBoxResult result = await MessageBox.ShowAsync(
            "정말 종료하시겠습니까?\n확인?",
            "종료 확인",
            MsgBoxButtons.YesNo,
            MsgBoxIcon.Question);

        if (result == MsgBoxResult.Yes)
        {
            window.Close();
        }
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task ThemeChangeAsync(Button? button)
    {
        if (button == null)
        {
            return;
        }
        //
        // MainViewModel a = await DI.GetAsync<MainViewModel>();
        //
        // LogHelper.Debug($"{a.GetType().Name}");
        //
        // return;

        // await LoadData();

        // Send("처리중...");
        // await Task.Delay(1000);
        // Send("처리 완료");

        // _configService
        //
        // bool isSave = await _configService.Save();


        // Theme = nameof(ThemeVariant.Dark);
        // ApplyTheme(Theme);
        // UpdateConfig();
        // AppConfigState configState = new()
        // {
        //     Theme = Theme,
        //     FontSize = FontSize,
        //     LastUserId = LastUserId,
        //     LastUserPassword = LastUserPassword
        // };
        //
        // await mAppConfigService.Save(configState);
        // LogHelper.Debug("AppService : AppConfig : Save ...");
        //
        // Send(new StatusMessageRegister("xxx"));

        // try
        // {
        //     Application app = Application.Current!;
        //     ThemeVariant? current = app.RequestedThemeVariant;
        //
        //     if (current == ThemeVariant.Light)
        //         app.RequestedThemeVariant = ThemeVariant.Dark;
        //     else if (current == ThemeVariant.Dark)
        //         app.RequestedThemeVariant = CustomThemes.DeepBlue;
        //     else if (current == CustomThemes.DeepBlue)
        //         app.RequestedThemeVariant = CustomThemes.MsWordLight;
        //     else if (current == CustomThemes.MsWordLight)
        //         app.RequestedThemeVariant = CustomThemes.MsWordDark;
        //     else
        //         app.RequestedThemeVariant = ThemeVariant.Light;
        // }
        // catch (Exception ex)
        // {
        //     LogHelper.Error("ThemeChange Error: " + ex.Message);
        // }

        await Task.CompletedTask;
    }
}