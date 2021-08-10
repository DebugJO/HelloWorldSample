// Console APP

using System;
using System.Reflection;

namespace ConsoleApp
{
    internal static class Program
    {
        private static void Main()
        {
            try
            {
                var dllFile = AppDomain.CurrentDomain.BaseDirectory + @"TestLibrary.dll";
                var assembly = Assembly.LoadFile(dllFile);
                var type = assembly.GetType("TestLibrary.HelloWorld"); // namespace.class

                // if (type == null) return;
                var obj = Activator.CreateInstance(type);
                var method = type.GetMethod("Hello"); // Method

                var param = new object[] { "가나닭" };
                var result = method?.Invoke(obj, param);
                Console.WriteLine(result?.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
