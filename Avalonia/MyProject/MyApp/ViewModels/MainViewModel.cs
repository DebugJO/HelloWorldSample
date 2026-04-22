using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MyApp.Controls;
using MyApp.Messages;
using MyApp.StateModels;
using MyApp.Views;
using MyApp.WindowHelper;
using MyApp.WindowHelper.ThemeHelper;
using MyAppLib.Helpers;
using System;
using System.ComponentModel;
using System.Threading;
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
    public partial string Theme { get; set; }

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
        WindowState = _mainState.WindowState;
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
        LogHelper.Info($"Message : StatusMessage changed : {value}");
#endif
    }

    partial void OnWindowStateChanged(WindowState value)
    {
        _mainState.WindowState = value;
#if DEBUG
        LogHelper.Info($"Message : WindowState changed : {value}");
#endif
    }

    partial void OnThemeChanged(string value)
    {
        _mainState.Theme = value;
        ThemeManager.ApplyTheme(Theme);

#if DEBUG
        LogHelper.Info($"Message : Theme changed : {value}");
#endif
    }

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
            const int timeoutSeconds = 10;
            Task appStartTask = AppStart();
            Task timeoutTask = Task.Delay(TimeSpan.FromSeconds(timeoutSeconds));
            Task completedTask = await Task.WhenAny(appStartTask, timeoutTask);

            if (completedTask == timeoutTask)
            {
                string errorMsg = $"시스템 시작 실패 : AppStart 작업이 {timeoutSeconds}초 안에 완료되지 않았습니다.";
                LogHelper.Error(errorMsg);
                SendState(errorMsg);
                return;
            }

            await appStartTask;
            _isStarted = true;
        }
        catch (Exception ex)
        {
            LogHelper.Error("AppStart Error : " + ex.Message);
            SendState("프로그램 시작 중 오류가 발생했습니다.");
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
            MainView mainView = DI.Get<MainView>();
            mainView.WindowState = WindowState.Normal;
            ThemeManager.ApplyTheme(Theme);
            mainView.Opacity = 1;

            SendState("데이터베이스 연결 중 ...");
            await Task.Delay(500);
            SendState("데이터베이스 연결 완료");
            LogHelper.Debug("AppStart : 데이터베이스 연결 완료");

            SendState("MQTT 브로커 접속 중...");
            await Task.Delay(500);
            SendState(" MQTT 브로커 접속 완료");
            LogHelper.Debug("AppStart : MQTT 브로커 접속 완료");

            SendState("서비스 시작 완료");
            LogHelper.Debug("AppStart : 서비스 시작 완료");
        }
        catch (Exception ex)
        {
            LogHelper.Error($"AppStart : 자원 할당 시작 중 오류 : {ex.Message}");
            throw new Exception(ex.Message);
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
            await AppStop();
        }
        catch (Exception ex)
        {
            LogHelper.Error("AppStop Error : " + ex.Message);
        }
        finally
        {
            _isForceClose = true;
            _isBusy = false;
            // RequestClose();
            Dispatcher.UIThread.Post(RequestClose);
        }
    }

    private async Task AppStop()
    {
        LogHelper.Debug("========== 프로그램 종료 Start ==========");
        LogHelper.Debug("AppStop : 자원 해재 시작 ...");

        using CancellationTokenSource cts = new(TimeSpan.FromSeconds(3));

        try
        {
            // Task dbTask = _dbService.CloseAsync(cts.Token);
            // Task mqttTask = _mqttService.DisconnectAsync(cts.Token);
            CancellationToken token = cts.Token;
            Task dbTask = Task.Run(async () =>
            {
                SendState("데이터베이스 자원 해제 중...");
                await Task.Delay(500, token);
            }, token);
            Task mqttTask = Task.Run(async () =>
            {
                SendState("MQTT 자원 해제 중...");
                await Task.Delay(500, token);
            }, token);
            await Task.WhenAny(Task.WhenAll(dbTask, mqttTask), Task.Delay(Timeout.Infinite, token));
            SendState("서비스 해제 완료");
            LogHelper.Debug("AppStop : 서비스 해제 완료");
        }
        catch (OperationCanceledException)
        {
            LogHelper.Warn("AppStop : 자원 해제 시간 초과로 인해 중단하고 로그만 기록합니다.");
        }
        catch (Exception ex)
        {
            LogHelper.Error($"AppStop : 자원 해제 중 오류 : {ex.Message}");
        }
        finally
        {
            LogHelper.Flush();
        }
    }

    private void RequestClose()
    {
        LogHelper.Debug("AppStop : Shutdown 호출 : 윈도우 종료 요청");
        LogHelper.Flush();

        IClassicDesktopStyleApplicationLifetime desktop = DI.Desktop();

        // desktop1.Shutdown();
        //
        //  if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
        //  {
        //      return;
        //  }

        _isForceClose = true;
        desktop.Shutdown();
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
        // if (e.Source is not Visual visual || visual.GetVisualRoot() is not Window window)
        // {
        //     return;
        // }

        MainView window = DI.Get<MainView>();

        PointerPointProperties pointerProps = e.GetCurrentPoint(window).Properties;

        if (!pointerProps.IsLeftButtonPressed)
        {
            return;
        }

        if (e.ClickCount == 2)
        {
            WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;

            window.WindowState = WindowState;
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
            "정말 종료하시겠습니까?\n확인",
            "종료 확인",
            MsgBoxButtons.YesNo,
            MsgBoxIcon.Question);

        if (result == MsgBoxResult.Yes)
        {
            Result<bool> config = await _mainState.SaveAsync(_mainState);

            config.Match(
                b => { LogHelper.Info(b ? "AppConfig : 설정 파일 저장 완료" : "AppConfig : 설정 파일 저장 실패"); },
                () => { LogHelper.Warn("AppConfig : 설정 파일이 없어 기본값을 사용합니다."); },
                (error, _) => { LogHelper.Error($"{error}"); }
            );

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

        // Theme = ThemeManager.GetTheme(ETheme.Dark);
        // _mainState.Theme = ThemeManager.GetTheme(ETheme.MsWordDark);;
        // ThemeManager.ApplyTheme(ETheme.MsWordDark);
        // _mainState.Theme = ThemeManager.GetTheme(ETheme.MsWordDark);

        // _mainState.Theme = ThemeManager.GetName()

        // Theme = ThemeManager.Dark;

        // Result<bool> result = await _mainState.SaveAsync(_mainState);
        //
        // result.Match(
        //     b => { LogHelper.Info(b ? "AppConfig : 설정 파일 저장 완료" : "AppConfig : 설정 파일 저장 실패"); },
        //     () => { LogHelper.Warn("AppConfig : 설정 파일이 없어 기본값을 사용합니다."); },
        //     (error, _) => { LogHelper.Error($"{error}"); }
        // );


        //     
        // Result<MainState> loadResult = await _mainState.LoadAsync();
        //
        // loadResult.Match(
        //     state =>
        //     {
        //         WindowState = state.WindowState;
        //         Theme = state.Theme;
        //         FontSize = state.FontSize;
        //         LastUserId = state.LastUserId;
        //         LastUserPassword = state.LastUserPassword;
        //         UpdateConfig();
        //         LogHelper.Info("AppConfig : 설정을 성공적으로 불러왔습니다.");
        //     },
        //     () => { LogHelper.Warn("AppConfig : 설정 파일이 없어 기본값을 사용합니다."); },
        //     (error, _) => { LogHelper.Error($"AppConfig : 설정 파일 불러오기 에러 : ({error}"); }
        // );

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

        Theme = ThemeManager.Light;

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