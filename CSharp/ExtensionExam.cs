using System;
using System.Text;

namespace ExtensionExam
{
    public static class ExMethod
    {
        public static string ToChangeCase(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var ch in str)
            {
                if (ch >= 'A' && ch <= 'Z')
                    sb.Append((char) ('a' + ch - 'A'));
                else if (ch >= 'a' && ch <= 'z')
                    sb.Append((char) ('A' + ch - 'a'));
                else
                    sb.Append(ch);
            }
            return sb.ToString();
        }

        public static bool IsFoundChar(this string str, char ch)
        {
            int pos = str.IndexOf(ch);
            return pos >= 0;
        }

        public static bool IsFoundString(this string str, string subStr, out int posStr)
        {
            int pos = str.IndexOf(subStr);
            posStr = pos;
            return pos >= 0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string str = "ABC 123 가나닭 def";

            Console.WriteLine($"{str.ToChangeCase()}");

            Console.WriteLine($"{str.IsFoundChar('e')}");

            if (str.IsFoundString("가나닭", out int pos))
                Console.WriteLine($"{pos}");
        }
    }
}
