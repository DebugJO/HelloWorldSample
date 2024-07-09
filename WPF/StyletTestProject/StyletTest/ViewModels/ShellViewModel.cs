using System.Linq;
using Stylet;
using StyletTest.AppConfig;
using StyletTest.Helpers;
using StyletTest.Views;
using System.Threading.Tasks;
using System.Windows;

namespace StyletTest.ViewModels;

public class ShellViewModel : Conductor<IScreen>.Collection.OneActive
{
    private readonly IWindowManager mWindowManager;
    private string mName;

    public ShellViewModel(IWindowManager windowManager)
    {
        mWindowManager = windowManager;
        mName = string.Empty;
        DisplayName = "SSS";
    }

    public string Name
    {
        get => mName;
        set => SetAndNotify(ref mName, value);
    }

    public async Task SayHello(object obj)
    {
        await Task.Delay(50);
        string a = IoC.Get<ShellViewModel>().Name;
        LogHelper.Logger.Debug($"11111111111111111 : {a} : {obj}");
        MessageBoxResult box = mWindowManager.ShowMessageBox($"Hello, {a} : {obj}", "테스트", MessageBoxButton.OKCancel);
        LogHelper.Logger.Debug(box == MessageBoxResult.OK ? "OK" : "Cancel");
    }

    public static async Task ButtonOnClick(object sender, RoutedEventArgs e)
    {
        await Task.Delay(50);
        LogHelper.Logger.Debug($"22222222222222222 : {IoC.Get<ShellView>().Title} : {sender} : {e}");
        //IoC.Get<ShellView>().Close();
    }
}