namespace DelegateExam
{
    internal class Program
    {
        private static readonly ShoppingCartModel cart = new();

        public static void Main()
        {
            PopulateCartWithDemoData();

            Console.WriteLine($"The total for the cart is {cart.GenerateTotal(SubTotalAlert, CalculateDiscountedTotal, AlertUser):C2}");
            Console.WriteLine();

            decimal total = cart.GenerateTotal(
                (subTotal) => Console.WriteLine($"The subtoal for cart 2 is {subTotal:C2}"),
                (products, subTotal) =>
                    {
                        if (products.Count > 3)
                        {
                            return subTotal * 0.5M;
                        }
                        else
                        {
                            return subTotal;
                        }
                    },
                (message) => Console.WriteLine($"Cart 2 Alert: {message}")
            );

            Console.WriteLine($"The total for cart 2 is {total:C2}");

            Console.WriteLine();
            Console.Write("Please press any key to exit the application...");
            Console.ReadKey();
        }

        private static void SubTotalAlert(decimal subTotal)
        {
            Console.WriteLine($"The subtotal is {subTotal:C2}");
        }

        private static void AlertUser(string message)
        {
            Console.WriteLine(message);
        }

        private static decimal CalculateDiscountedTotal(List<ProductModel> items, decimal subTotal)
        {
            if (subTotal > 100)
            {
                return subTotal * 0.80M;
            }
            else if (subTotal > 50)
            {
                return subTotal * 0.85M;
            }
            else if (subTotal > 10)
            {
                return subTotal * 0.90M;
            }
            else
            {
                return subTotal;
            }
        }

        private static void PopulateCartWithDemoData()
        {
            cart.Items.Add(new ProductModel { ItemName = "AAA", Price = 3.63M });
            cart.Items.Add(new ProductModel { ItemName = "BBB", Price = 2.95M });
            cart.Items.Add(new ProductModel { ItemName = "CCC", Price = 7.51M });
            cart.Items.Add(new ProductModel { ItemName = "DDD", Price = 8.84M });
        }
    }
}