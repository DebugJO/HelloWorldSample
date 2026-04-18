using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly MainState _state;
    // private readonly AppConfigState _configState;
    // private readonly IConfigService _configService;

    private bool _isStarted;
    private bool _isForceClose;
    private bool _isBusy;

    /*  AppConfigState Property */
    // [ObservableProperty]
    // public partial string Theme { get; set; }
    //
    // [ObservableProperty]
    // public partial double FontSize { get; set; }
    //
    // [ObservableProperty]
    // public partial string LastUserId { get; set; }
    //
    // [ObservableProperty]
    // public partial string LastUserPassword { get; set; }

    /* MainState Property */
    [ObservableProperty]
    public partial string StatusMessage { get; set; }

    public ObservableCollection<string> Items { get; }

    /* MainViewModel Property */
    [ObservableProperty]
    public partial object? CurrentPage { get; set; }

    public MainViewModel(MainState state)
    {
        // _configService = configService;

        _state = state;
        // _configState = configState;

        // Theme = _configState.Theme;
        // FontSize = _configState.FontSize;
        // LastUserId = _configState.LastUserId;
        // LastUserPassword = _configState.LastUserPassword;

        StatusMessage = _state.StatusMessage;
        Items = _state.Items;
        RegisterStatusMessage();
        // CurrentPage =  DI.Get<Sub1ViewModel>();
    }

    private void RegisterStatusMessage()
    {
        WeakReferenceMessenger.Default.Register<StatusMessageRegister>(
            this, (_, m) => { UpdateStatus(m.Text); });
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
            _state.StatusMessage = value;
        }
#if DEBUG
        LogHelper.Info($"StatusMessage changed: {value}");
#endif
    }

    // private void UpdateConfig(AppConfigState configState)
    // {
    //     Theme = configState.Theme;
    //     _configState.Theme = Theme;
    //
    //     FontSize = configState.FontSize;
    //     _configState.FontSize = FontSize;
    //
    //     LastUserId = configState.LastUserId;
    //     _configState.LastUserId = LastUserId;
    //
    //     LastUserPassword = configState.LastUserPassword;
    //     _configState.LastUserPassword = LastUserPassword;
    // }

    // public void ClickMenu2() => CurrentPage = DI.Get<Sub1ViewModel>();
    // public void ClickMenu2() => CurrentPage = DI.Get<Sub2ViewModel>();
    // StackPanel>
    // <Button Content="서버 1" Command="{Binding ClickMenu1}" />
    // <Button Content="서버 2" Command="{Binding ClickMenu2}" />
    // <Button Content="서버 3" Command="{Binding ClickMenu3}" />
    // </StackPanel>


    // private async Task LoadData()
    // {
    //     try
    //     {
    //         UpdateStatus("Loading Data...");
    //
    //         List<string> items = await LoadItems();
    //
    //         Items.Clear();
    //
    //         foreach (string item in items)
    //         {
    //             Items.Add(item);
    //         }
    //
    //         UpdateStatus("Loaded Data");
    //     }
    //     catch (Exception ex)
    //     {
    //         LogHelper.Error($"Load Test Error : {ex.Message}");
    //     }
    // }
    //
    // private async Task<List<string>> LoadItems()
    // {
    //     await Task.Delay(1000);
    //     return ["hello", "world"];
    // }

    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task AppStartAsync()
    {
        if (_isStarted)
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
            _isStarted = true;
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
        LogHelper.Debug("Desktop : RequestClose : 윈도우 종료 요청");

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
            // AppConfigState configState = await _configService.Load();
            // ApplyTheme(configState.Theme);
            // UpdateConfig(configState);

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
    }

    // private void ApplyTheme(string themeName)
    // {
    //     Application app = Application.Current!;
    //     ThemeVariant? current = app.RequestedThemeVariant;
    //
    //     const string light = nameof(ThemeVariant.Light);
    //     const string dark = nameof(ThemeVariant.Dark);
    //     const string deepBlue = nameof(CustomThemes.DeepBlue);
    //     const string mswordLight = nameof(CustomThemes.MsWordLight);
    //     const string mswordDark = nameof(CustomThemes.MsWordDark);
    //
    //     if (current is not null)
    //     {
    //         app.RequestedThemeVariant = themeName switch
    //         {
    //             light => ThemeVariant.Light,
    //             dark => ThemeVariant.Dark,
    //             deepBlue => CustomThemes.DeepBlue,
    //             mswordLight => CustomThemes.MsWordLight,
    //             mswordDark => CustomThemes.MsWordDark,
    //             _ => app.RequestedThemeVariant
    //         };
    //     }
    // }

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

        LogHelper.Debug("AppStop : 자원 해제 완료");
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
        // await _configService.Save(_configState);
        // ApplyTheme(_configState.Theme);
        // UpdateConfig(_configState);

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