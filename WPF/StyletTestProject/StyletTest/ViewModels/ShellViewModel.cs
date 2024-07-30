using System;
using Stylet;
using StyletTest.AppConfig;
using StyletTest.Views;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using StyletTest.Helpers;

namespace StyletTest.ViewModels;

public class ShellViewModel : Conductor<IScreen>.Collection.OneActive
{
    //private readonly IWindowManager mWindowManager;
    private string mName;

    public ShellViewModel()//(IWindowManager windowManager)
    {
        //mWindowManager = windowManager;
        mName = string.Empty;
        DisplayName = "SSS";
    }

    public string Name
    {
        get => mName;
        set => SetAndNotify(ref mName, value);
    }

    public void ShowMenuFirst()
    {
        ActiveItem = IoC.Get<FirstViewModel>();
    }

    public void ShowMenuSecond()
    {
        ActiveItem = IoC.Get<SecondViewModel>();
    }

    public async Task ButtonOnTest(object sender, RoutedEventArgs e)
    {
        if (sender is not Button btn)
        {
            return;
        }

        try
        {
            btn.IsEnabled = false;
            await Task.Delay(200);
            await Execute.OnUIThreadAsync(() => { Name = "동작 중..."; });
            await Task.Delay(200);
            await Execute.OnUIThreadAsync(() => { Name = string.Empty; });
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"ButtonOnTest ERROR : : {ex.Message}"); ;
        }
        finally
        {
            btn.IsEnabled = true;
        }
    }

    public async Task ButtonOnClose(object sender, RoutedEventArgs e)
    {
        await Execute.OnUIThreadAsync(() => { Name = "종료 중 입니다"; });
        await Task.Delay(200);
        IoC.Get<ShellView>().Close();
    }

    //public async Task SayHello(object obj)
    //{
    //    await Task.Delay(50);
    //    string a = IoC.Get<ShellViewModel>().Name;
    //    LogHelper.Logger.Debug($"11111111111111111 : {a} : {obj}");
    //    //MessageBoxResult box = mWindowManager.ShowMessageBox($"Hello, {a} : {obj}", "테스트", MessageBoxButton.OKCancel);
    //    //LogHelper.Logger.Debug(box == MessageBoxResult.OK ? "OK" : "Cancel");
    //}

    //public static async Task ButtonOnClick(object sender, RoutedEventArgs e)
    //{
    //    await Task.Delay(50);
    //    LogHelper.Logger.Debug($"22222222222222222 : {IoC.Get<ShellView>().Title} : {sender} : {e}");
    //    //IoC.Get<ShellView>().Close();
    //    WindowHelper.SendMessage("가나닭햏펲");
    //}
}