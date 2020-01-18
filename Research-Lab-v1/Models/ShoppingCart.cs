using System.Collections.Generic;
using System.Linq;

namespace Research_Lab_v1.Models
{
    public class ShoppingCart
    {
        //public double OrderTotal
        //{
        //    get { return CartItems == null ? 0 : CartItems.Sum(item => (item.Product.SalePrice*item.Quantity)); }
        //}
        //public long Id { get; set; }
        public IList<CartItem> CartItems { get; set; }
    }
}