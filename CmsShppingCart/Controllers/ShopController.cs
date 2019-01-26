using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsShppingCart.Models.ViewModels.Shop;
using CmsShppingCart.Models.Data;

namespace CmsShppingCart.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return RedirectToAction("Index","Pages");
        }

        //GET: CategoryMenuPartial
        public ActionResult CategoryMenuPartial()
        {
            //Declare list of category
            List<CategoryVM> CategoryVMLis;
            //init the list
            using (Db db=new Db())
            {
                CategoryVMLis = db.Categories.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoryVM(x)).ToList();
            }
            //return partial with list
            return PartialView(CategoryVMLis);
        }

        public ActionResult Category(string name)
        {
            //Declare Product List
            List<ProductVM> ProductVMList;

            //get ProductVMList
            using (Db db = new Db())
            {
                ProductVMList = db.Products.ToArray().Where(x => x.CategoryName.Equals(name)).Select(x => new ProductVM(x)).ToList();
            }

            //retern view with list
            return View(ProductVMList);
        }
    }
}