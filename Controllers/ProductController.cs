using QLBH.Models;
using System.Linq;
using System.Web.Mvc;

namespace QLBH.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        private NWDataClassesDataContext da = new NWDataClassesDataContext();

        public ActionResult Index()
        {
            var p = da.Products.Select(s => s).ToList();
            return View(p);
        }

        public ActionResult ProductList()
        {
            var p = da.Products.Select(s => s).ToList();
            return View(p);
        }

        public ActionResult Create()
        {
            var p = da.Categories.Select(s => s);
            ViewData["LoaiSP"] = new SelectList(da.Categories, "CategoryID", "CategoryName");
            ViewData["LoaiNCC"] = new SelectList(da.Suppliers, "SupplierID", "CompanyName");
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection, Product product)
        {
            var tensp = collection["ProductName"];


            if (string.IsNullOrEmpty(tensp))
            {
                ViewData["loi"] = "hãy nhập tên sản phẩm";
            }
            else
            {
                product.CategoryID = int.Parse(collection["LoaiSP"]);
                product.SupplierID = int.Parse(collection["LoaiNCC"]);
                da.Products.InsertOnSubmit(product);
                da.SubmitChanges();
                return RedirectToAction("ProductList");
            }
            return this.Create();
        }

        public ActionResult Edit(int id)
        {
            var sp = da.Products.First(m => m.ProductID == id);
            return View(sp);
        }



        [HttpPost]
        public ActionResult Edit(FormCollection collection, int id)
        {
            var sp = da.Products.First(m => m.ProductID == id);

            sp.ProductName = collection["ProductName"];
            sp.SupplierID = int.Parse(collection["SupplierID"]);
            sp.UnitPrice = decimal.Parse(collection["UnitPrice"]);
            sp.UnitsInStock = short.Parse(collection["UnitsInStock"]);

            UpdateModel(sp);
            da.SubmitChanges();
            return RedirectToAction("ProductList");
        }


        public ActionResult Details(int id)
        {
            var sp = da.Products.Where(s => s.ProductID == id).FirstOrDefault();
            return View(sp);
        }


        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var sp = da.Products.First(m => m.ProductID == id);
            da.Products.DeleteOnSubmit(sp);
            da.SubmitChanges();
            return RedirectToAction("ProductList");
        }

        public ActionResult Delete(int id)
        {
            var sp = da.Products.First(m => m.ProductID == id);
            return View(sp);
        }

    }
}