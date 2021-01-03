using System.Linq;
using System;
using System.Collections.Generic;

namespace delegate_exam
{
    internal class Program
    {
        private static void Login(string str)
        {
            Console.WriteLine("로그인 : " + str);
        }

        private static void Logout(string str)
        {
            Console.WriteLine("로그아웃 : " + str);
        }

        static Dictionary<string, Action<string>> actions = new Dictionary<string, Action<string>>()
        { { "login", Login }, { "logout", Logout }
        };

        static void Main(string[] args)
        {
            actions["login"]("가나닭");
            actions["logout"]("마바삵");
        }
    }
}
