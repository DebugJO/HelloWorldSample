using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDI
{
    public class WebNotification : INotificationService
    {
        public void NotifyUserNameChanged(User user)
        {
            Console.WriteLine($"Web - UserName has been changed to : {user.UserName}");
        }
    }
}
