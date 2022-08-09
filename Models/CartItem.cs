using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLBH.Models;

namespace QLBH.Models
{
    public class CartItem
    {
        private NWDataClassesDataContext da = new NWDataClassesDataContext();

        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal? Total { get { return UnitPrice * Quantity; } }
        public CartItem(int ProductID)
        {
            this.ProductID = ProductID;
            var product = da.Products.Single(n => n.ProductID == ProductID);
            ProductName = product.ProductName;
            UnitPrice = product.UnitPrice;
            Quantity = 1;
        }
    }
}