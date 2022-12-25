using MvvmExample.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MvvmExample.Services;

public interface IUserInfoService
{
    Task<List<UserInfo>> GetUserInfoList(UserInfo userInfo);
}