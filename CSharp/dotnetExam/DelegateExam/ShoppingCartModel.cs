using System;
using System.Collections.Generic;
using System.Linq;

namespace DelegateExam
{
    public class ShoppingCartModel
    {
        public delegate void MentionDiscount(decimal subTotal);

        public List<ProductModel> Items { get; set; } = new List<ProductModel>();

        public decimal GenerateTotal(MentionDiscount mentionDiscount, Func<List<ProductModel>, decimal, decimal> calculateDiscountTotal, Action<string> tellDiscount)
        {
            decimal subTotal = Items.Sum(x => x.Price);

            mentionDiscount(subTotal);

            tellDiscount("We are applying your discount.");

            return calculateDiscountTotal(Items, subTotal);
        }
    }
}