using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MyApp.Controls;
using MyApp.Messages;
using MyApp.StateModels;
using MyApp.WindowHelper;
using MyApp.WindowHelper.ThemeHelper;
using MyAppLib.Helpers;
using System;
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
        LogHelper.Debug($"Message : StatusMessage changed : {value}");
#endif
    }

    partial void OnWindowStateChanged(WindowState value)
    {
        _mainState.WindowState = value;
#if DEBUG
        LogHelper.Debug($"Message : WindowState changed : {value}");
#endif
    }

    partial void OnThemeChanged(string value)
    {
        _mainState.Theme = value;
#if DEBUG
        LogHelper.Debug($"Message : Theme changed : {value}");
#endif
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task AppStartAsync(RoutedEventArgs e)
    {
        if (_isStarted)
        {
            return;
        }

        try
        {
            const int timeoutSeconds = 10;
            Task appStartTask = AppStart(e);
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

    private async Task AppStart(RoutedEventArgs e)
    {
        LogHelper.Debug("AppStart : 자원 할당 시작 ...");

        try
        {
            if (e.GetWindow() is not { } mainView)
            {
                return;
            }

            await LoadConfig();

            ThemeManager.ApplyTheme(Theme);
            mainView.WindowState = WindowState;

            Dispatcher.UIThread.Post(() =>
            {
                Dispatcher.UIThread.Post(() =>
                {
                    mainView.ExtendClientAreaToDecorationsHint = true;
                    mainView.Opacity = 1;
                    mainView.SystemDecorations = SystemDecorations.Full;
                }, DispatcherPriority.Render);
            }, DispatcherPriority.ApplicationIdle);

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

    private async Task LoadConfig()
    {
        Result<MainState> loadResult = await _mainState.LoadAsync();

        loadResult.Match(
            state =>
            {
                WindowState = state.WindowState;
                Theme = state.Theme;
                LastUserId = state.LastUserId;
                LastUserPassword = state.LastUserPassword;
                LogHelper.Info("AppConfig : 설정을 성공적으로 불러왔습니다.");
            },
            () => { LogHelper.Warn("AppConfig : 설정 파일이 없어 기본값을 사용합니다."); },
            (error, _) => { LogHelper.Error($"{error}"); }
        );
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task AppClosingAsync(WindowClosingEventArgs e)
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
            CancellationToken token = cts.Token;

            Task dbTask = Task.Run(async () =>
            {
                // dbService.CloseAsync(cts.Token);
                SendState("데이터베이스 자원 해제 중...");
                await Task.Delay(500, token);
            }, token);
            Task mqttTask = Task.Run(async () =>
            {
                // _mqttService.DisconnectAsync(cts.Token);
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

        _isForceClose = true;
        DI.Desktop()?.Shutdown();
    }

    [RelayCommand]
    private void TitleBarAction(PointerPressedEventArgs e)
    {
        if (e.GetWindow() is not { } window)
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
                b => { LogHelper.Debug(b ? "AppConfig : 설정 파일 저장 완료" : "AppConfig : 설정 파일 저장 실패"); },
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

        ThemeManager.ApplyTheme(ThemeManager.Dark);
        Theme = ThemeManager.Dark;
        await Task.CompletedTask;
    }
}