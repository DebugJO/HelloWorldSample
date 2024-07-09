using System;
using System.Threading.Tasks;

namespace StyletTest.Helpers;

public static class ExtensionHelper
{
    public static async void AWait(this Task task, Action? completedCallback, Action<Exception>? errorCallback)
    {
        try
        {
            await task;
            completedCallback?.Invoke();
        }
        catch (Exception exception)
        {
            errorCallback?.Invoke(exception);
        }
    }
}