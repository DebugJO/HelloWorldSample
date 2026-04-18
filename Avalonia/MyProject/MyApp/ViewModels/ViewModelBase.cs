using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MyApp.Messages;

namespace MyApp.ViewModels;

public class ViewModelBase : ObservableObject
{
    protected void Send<T>(T message) where T : class
    {
        WeakReferenceMessenger.Default.Send(message);
    }

    protected void Send(string message)
    {
        WeakReferenceMessenger.Default.Send(new StatusMessageRegister(message));
    }
}