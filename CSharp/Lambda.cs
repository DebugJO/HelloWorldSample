using System;
using System.Collections.Generic;
using System.Linq;

namespace LambdaExam
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            Func<int, bool> oddCheck = x => x % 2 != 0;

            Action<int> printOdd = x =>
            {
                var square = x * x;
                var cube = x * x * x;
                Console.WriteLine($"x:{x} : square:{square}, cube:{cube}");
            };

            var oddNumbers = numbers.Where(oddCheck).ToList();

            oddNumbers.ForEach(x => Console.WriteLine(x));

            oddNumbers.ForEach(printOdd);

            //Console.WriteLine(string.Concat((char)13, (char)10) + "Press any key to continue");
            Console.WriteLine(Environment.NewLine + "Press any key to continue");
            Console.ReadKey(true);
        }
    }
}
