using QLBH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLBH.Controllers
{
    public class CardController : Controller
    {
        NWDataClassesDataContext da = new NWDataClassesDataContext();

        private List<CartItem> getCarts()
        {
            var cart = Session["cart"] as List<CartItem>;

            if (cart == null)
            {
                cart = new List<CartItem>();
                Session["cart"] = cart;
            }

            return cart;
         }

        public ActionResult Index()
        {
            var cart = getCarts();
            if(cart.Count == 0)
            {
                return RedirectToAction("ProductList", "Product");
            }    

            return View(cart);
        }

        public ActionResult AddToCart(int productID)
        {
            List<CartItem> cart = getCarts();
            CartItem c = cart.Find(s => s.ProductID == productID);
            if (c == null)
            {
                c = new CartItem(productID);
                cart.Add(c);
            }
            else
                c.Quantity++;

            return RedirectToAction("Index");
        }

        public ActionResult RemoveCart(int productID)
        {
            var cart = getCarts();
            cart.RemoveAll(i => i.ProductID == productID);
            Session["cart"] = cart;

            return RedirectToAction("Index");
        }
    }
}