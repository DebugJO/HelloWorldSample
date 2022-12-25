using MvvmExample.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MvvmExample.Services;

public class UserInfoService : IUserInfoService
{
    public async Task<List<UserInfo>> GetUserInfoList(UserInfo userInfo)
    {
        _ = userInfo;

        return await Task.Run(async () =>
        {
            await Task.Delay(3000);
            return UserInfo.UserInfoList();
        });
    }
}
