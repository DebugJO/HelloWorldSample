using System;

namespace ConsoleDI
{
    public class ConsoleNotification : INotificationService
    {
        public void NotifyUserNameChanged(User user)
        {
            Console.WriteLine($"Console - UserName has been changed to : {user.UserName}");
        }
    }
}
