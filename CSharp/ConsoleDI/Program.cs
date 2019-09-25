using System;

namespace ConsoleDI
{
    class Program
    {
        static void Main()
        {
            var user1 = new User("홍길동", new ConsoleNotification());
            user1.ChangeUserName("홍길남");

            var user2 = new User("홍길서", new WebNotification());
            user2.ChangeUserName("홍길북");

            Console.ReadLine();
        }
    }
}
