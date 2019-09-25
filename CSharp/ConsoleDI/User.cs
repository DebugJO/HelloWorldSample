namespace ConsoleDI
{
    public class User
    {
        private readonly INotificationService mNotificationService;

        public string UserName { get; private set; }

        public User(string username, INotificationService notificationService)
        {
            UserName = username;
            mNotificationService = notificationService;
        }

        public void ChangeUserName(string newUserName)
        {
            UserName = newUserName;
            mNotificationService.NotifyUserNameChanged(this);
        }
    }
}
