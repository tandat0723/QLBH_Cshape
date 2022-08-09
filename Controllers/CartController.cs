using QLBH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace QLBH.Controllers
{
    public class CartController : Controller
    {
        NWDataClassesDataContext da = new NWDataClassesDataContext();

        private List<CartItem> getCarts()
        {
            List<CartItem> cart = Session["CartItem"] as List<CartItem>;

            if (cart == null)
            {
                cart = new List<CartItem>();
                Session["CartItem"] = cart;
            }

            return cart;
         }

        public ActionResult ListCarts()
        {
            List<CartItem> cart = getCarts();

            if(cart.Count == 0)
            {
                return RedirectToAction("ProductList", "Product");
            }
            ViewBag.CountProduct = Count();
            ViewBag.Total = Total();

            return View(cart);
        }

        public ActionResult AddToCart(int id)
        {
            List<CartItem> cart = getCarts();
            CartItem c = cart.Find(s => s.ProductID == id);
            if (c == null)
            {
                c = new CartItem(id);
                cart.Add(c);
            }
            else
                c.Quantity++;

            return RedirectToAction("ListCarts");
        }

        public ActionResult RemoveCart(int productID)
        {
            var cart = getCarts();
            cart.RemoveAll(i => i.ProductID == productID);
            Session["CartItem"] = cart;

            return RedirectToAction("ListCarts");
        }

        private int Count()
        {
            int n = 0;
            List<CartItem> cart = Session["CartItem"] as List<CartItem>;
            if (cart != null)
                n = cart.Sum(s => s.Quantity);

            return n;
        }

        private decimal? Total()
        {
            decimal? total = 0;
            List<CartItem> cart = Session["CartItem"] as List<CartItem>;
            if (cart != null)
                total = cart.Sum(s => s.Total);

            return total;
        }

        public ActionResult OrderProduct(FormCollection fCollection)
        {
            using (TransactionScope tranScope = new TransactionScope())
            {
                try
                {
                    Order order = new Order();

                    order.OrderDate = DateTime.Now;
                    da.Orders.InsertOnSubmit(order);
                    da.SubmitChanges();
                    //order = da.Orders.OrderByDescending(s => s.OrderID).Take(1).SingleOrDefault();

                    List<CartItem> carts = getCarts();
                    foreach (var item in carts)
                    {
                        Order_Detail d = new Models.Order_Detail();
                        d.OrderID = order.OrderID;
                        d.ProductID = item.ProductID;
                        d.Quantity = short.Parse(item.Quantity.ToString());
                        d.UnitPrice = decimal.Parse(item.UnitPrice.ToString());
                        d.Discount = 0;

                        da.Order_Details.InsertOnSubmit(d);
                    }
                    da.SubmitChanges();
                    tranScope.Complete();
                    Session["CartItem"] = null;
                    return RedirectToAction("OrderDetailList");
                } 
                catch(Exception)
                {
                    tranScope.Dispose();
                    return RedirectToAction("ProductList", "Product");
                }
            }
        }

        public ActionResult OrderDetailList()
        {
            return View();
        }
    }
}