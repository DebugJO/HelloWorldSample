using System;
using System.Threading.Tasks;

namespace MyAppLib.Helpers;

public static class SyncToAsyncExtensions
{
    // 리턴값이 없는 동기 함수용 (Action)
    public static async Task<bool> RunAsync(this Action syncAction, Action<Exception>? errorCallback = null)
    {
        try
        {
            await Task.Run(syncAction).ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            errorCallback?.Invoke(ex);
            return false;
        }
    }

    // 리턴값이 있는 동기 함수용 (Func<T>)
    public static async Task<T?> RunAsync<T>(this Func<T> syncFunc, Action<Exception>? errorCallback = null)
    {
        try
        {
            return await Task.Run(syncFunc).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            errorCallback?.Invoke(ex);
            return default;
        }
    }
}

// 리턴값이 없는 경우 (Action)
// private async Task AsyncSaveProcess()
// {
//     Action saveAction = () => SyncUpdateConfig();
//     bool isSuccess = await saveAction.RunAsync(ex => LogHelper.Error($"저장 실패: {ex.Message}"));
//
//     if (isSuccess)
//     {
//         LogHelper.Info("성공적으로 저장되었습니다.");
//     }
//     else
//     {
//         LogHelper.Info("저장 중 문제가 발생했습니다.");
//     }
// }

// private async Task AsyncFetchProcess()
// {
//     Func<string> fetchFunc = () => SyncGetDatabaseStatus();
//     string? status = await fetchFunc.RunAsync(ex => LogHelper.Error($"조회 실패: {ex.Message}"));
//     if (status != null)
//     {
//         LogHelper.Info($"현재 상태: {status}");
//     }
// }

// private async Task AsyncTest()
// {
//     bool isOk = await Task.Run(() =>
//     {
//         try
//         {
//             SyncUpdateConfig();
//             return true;
//         }
//         catch (Exception ex)
//         {
//             LogHelper.Error(ex.Message);
//             return false;
//         }
//     });
//     
//     if (!isOk)
//     {
//         LogHelper.Info("xxxxxxxxxxxxxxxxx");
//     }
// }