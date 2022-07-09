namespace DelegateExam
{
    public class ShoppingCartModel
    {
        //public delegate void MentionDiscount(decimal subTotal);

        public List<ProductModel> Items { get; set; } = new();

        // public decimal GenerateTotal(MentionDiscount mentionDiscount)
        public decimal GenerateTotal(Action<decimal> mentionDiscount, Func<List<ProductModel>, decimal, decimal> calculateDiscountedTotal, Action<string> tellUserWeAreDiscounting)
        {
            decimal subTotal = Items.Sum(x => x.Price);

            mentionDiscount(subTotal);

            tellUserWeAreDiscounting("We are applying you discount.");

            return calculateDiscountedTotal(Items, subTotal);
        }
    }
}
