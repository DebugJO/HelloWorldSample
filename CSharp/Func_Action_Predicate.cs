using System;

namespace ConsoleExam
{
    internal class Program
    {
        // Reference
        // 10 Clicks - Free Software Engineer Training, "Func vs. Action vs. Predicate(https://www.youtube.com/watch?v=XJCTw9AaMXc)"
        //

        private static void Main()
        {
            // [1], [2], [3]
            //Console.WriteLine(Product.Cost(5, MyDiscount));

            // [4]
            // Console.WriteLine(Product.Cost(5, MyDiscount, CheckInStock));

            // [5]
            Console.WriteLine(Product.Cost(5, MyDiscount, CheckInStock, GiftWrap));
        }

        // [1], [2]
        //private static decimal MyDiscount()
        //{
        //    return 1M;
        //}

        // [3], [4]
        private static decimal MyDiscount(int count)
        {
            if (count == 1)
            {
                return 0.5M;
            }

            return 1M;
        }

        // [4], [5]
        private static void CheckInStock()
        {
            Console.WriteLine("Checking in stock...");
        }

        // [5]
        private static bool GiftWrap(int count)
        {
            return count >= 100;
        }
    }

    public class Product
    {
        // [1]
        //public delegate decimal DiscountValue();
        //public static decimal Cost(int total, DiscountValue discount)
        //{
        //    return total * discount.Invoke();
        //}

        // [2]
        //public static decimal Cost(int total, Func<decimal> discount)
        //{
        //    return total * discount.Invoke();
        //}

        // [3]
        //public static decimal Cost(int total, Func<int, decimal> discount)
        //{
        //    return total * discount.Invoke(1);
        //}

        // [4]
        //public static decimal Cost(int total, Func<int, decimal> discount, Action checkInStock)
        //{
        //    checkInStock.Invoke();

        //    return total * discount.Invoke(1);
        //}

        // [5]
        public static decimal Cost(int total, Func<int, decimal> discount, Action checkInStock, Predicate<int> giftWrap)
        {
            checkInStock.Invoke();

            if (giftWrap.Invoke(100))
            {
                Console.WriteLine("Thanks for buying!!!");
            }

            return total * discount.Invoke(1);
        }
    }
}
