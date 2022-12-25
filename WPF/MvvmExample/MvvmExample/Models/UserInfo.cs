using System.Collections.Generic;

namespace MvvmExample.Models;

public class UserInfo
{
    public string ID { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    // 아래의 내용은 모델에 작성하는 것이 아닌 ORM 등 데이터베이스에서 가져온다고 가정
    public static List<UserInfo> UserInfoList()
    {
        List<UserInfo> userInfoList = new()
        {
            new UserInfo(){ID = "A1001", Name = "관리자"},
            new UserInfo(){ID = "B1234", Name = "홍길동"}
        };

        return userInfoList;
    }
}
