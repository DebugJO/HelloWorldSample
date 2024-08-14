using Caliburn.Micro;
using CMTest80.Helpers;
using CMTest80.Views;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CMTest80.ViewModels;

public class ShellViewModel : Conductor<IScreen>.Collection.OneActive
{
    private string mTest;

    public ShellViewModel()
    {
        mTest = string.Empty;
        AppStartup.OnReceiveMessage += AppStartupOnOnReceiveMessage;
    }

    private void AppStartupOnOnReceiveMessage(object? sender, string data)
    {
        if (sender is not Window window)
        {
            return;
        }

        Test = $"{data} : {window}";
    }

    public string Test
    {
        get => mTest;
        set
        {
            mTest = value;
            NotifyOfPropertyChange();
        }
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
            await Task.Delay(50);
            WindowHelper.SendMessage("헬로우월드");
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"ButtonOnTest ERROR : {ex.Message}");
        }
        finally
        {
            btn.IsEnabled = true;
        }
    }

    public async Task ButtonOnWindowClose(object sender, RoutedEventArgs e)
    {
        if (sender is not Button btn)
        {
            return;
        }

        try
        {
            btn.IsEnabled = false;
            Test = "프로그램 : 종료 준비 중 .....";
            LogHelper.Logger.Info("===== 프로그램 : 종료 준비 중 .....");
            await Task.Delay(50);
            IoC.Get<ShellView>().Close();
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"ButtonOnWindowClose ERROR : {ex.Message}");
            Environment.Exit(0);
        }
    }
}