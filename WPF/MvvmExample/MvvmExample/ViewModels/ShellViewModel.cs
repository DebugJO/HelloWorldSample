using Caliburn.Micro;
using MvvmExample.Helpers;
using MvvmExample.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MvvmExample.ViewModels;

public class ShellViewModel : Conductor<IScreen>.Collection.OneActive
{
    private string mInfoMessage;
    private WindowState mWindowState;

    public ShellViewModel()
    {
        mInfoMessage = string.Empty;
        mWindowState = WindowState.Normal;
    }

    public string InfoMessage
    {
        get => mInfoMessage;
        set
        {
            mInfoMessage = value;
            NotifyOfPropertyChange();
        }
    }

    public WindowState WindowState
    {
        get
        {
            ShellView window = IoC.Get<ShellView>();

            if (mWindowState == WindowState.Maximized)
            {
                window.BorderThickness = new Thickness(8);
            }
            else
            {
                window.BorderThickness = new Thickness(0);
            }
            return mWindowState;
        }
        set
        {
            mWindowState = value;
            NotifyOfPropertyChange();
        }
    }

    public void DragMoveWindow(object sender, MouseButtonEventArgs e)
    {
        switch (e.ClickCount)
        {
            case 1 when e.ButtonState == MouseButtonState.Pressed:
                {
                    WindowHelper.DragMoveWindow();
                    e.Handled = true;
                    break;
                }
            case 2 when e.ButtonState == MouseButtonState.Pressed:
                WindowHelper.MaxMinChangeScreen(IoC.Get<ShellView>());
                e.Handled = true;
                break;
            default:
                e.Handled = true;
                break;
        }
    }

    public void ButtonSystem(object sender, RoutedEventArgs e)
    {
        if (sender is not Button btn)
        {
            return;
        }

        ShellView window = IoC.Get<ShellView>();

        switch (btn.Tag.ToString())
        {
            case "Full":
                {
                    WindowHelper.WindowFullScreen(window);
                    break;
                }
            case "MaxNor":
                {
                    if (window.WindowState == WindowState.Normal)
                    {
                        WindowHelper.WindowMax(window);
                    }
                    else
                    {
                        WindowHelper.WindowNormal(window);
                    }
                    break;
                }
            case "Min":
                {
                    WindowHelper.WindowMin(window);
                    break;
                }
            case "Close":
                {
                    IoC.Get<ShellView>().Close();
                    break;
                }
            default:
                break;
        }
    }

    public void ButtonMenu(object sender, RoutedEventArgs e)
    {
        if (sender is not Button btn)
        {
            return;
        }

        switch (btn.Tag.ToString())
        {
            case "First":
                {
                    ActiveItem = IoC.Get<FormFirstViewModel>();
                    break;
                }
            case "Second":
                {
                    ActiveItem = IoC.Get<FormSecondViewModel>();
                    break;
                }
            default:
                break;
        }
    }
}
