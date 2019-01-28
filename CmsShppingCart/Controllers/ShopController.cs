using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsShppingCart.Models.ViewModels.Shop;
using CmsShppingCart.Models.Data;
using System.IO;

namespace CmsShppingCart.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return RedirectToAction("Index","Pages");
        }

        //GET: /Shop/Category/name
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
            //Declare a list of ProductVM
            List<ProductVM> ProductVMList;
            using (Db db = new Db())
            {
                //Get category id
                CategoryDTO oCatDTO = db.Categories.Where(x => x.Slug == name).FirstOrDefault();
                int CatID = oCatDTO.Id;

                //Init the list
                ProductVMList = db.Products.ToArray().Where(x => x.CategoryId==CatID).Select(x => new ProductVM(x)).ToList();

                //Get category name
                var ProductCat = db.Products.Where(x => x.CategoryId == CatID).FirstOrDefault();
                ViewBag.CategoryName = ProductCat.CategoryName;

            }

            //retern view with list
            return View(ProductVMList);
        }

        [HttpGet]
        [ActionName("Product-Details")]
        //GET : Shop/Product-Details/name
        public ActionResult ProductDetails(string name)
        {
            //Declare ProductVM and ProductDTO
            ProductVM model;
            ProductDTO oProdDTO;

            //Init Product ID
            int ProdID;

            using (Db db = new Db())
            {
                //Check If Product Exists
                if (!db.Products.Any(x=>x.Slug.Equals(name)))
                {
                    return RedirectToAction("Index", "Shop");
                }

                //Init ProductDTO
                oProdDTO = db.Products.Where(x => x.Slug.Equals(name)).FirstOrDefault();

                //Get Inserted ID
                ProdID = oProdDTO.Id;

                //Init Model
                model = new ProductVM(oProdDTO);

                //Get Gallery Images
                model.GallaryImage = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + ProdID + "/Gallery/Thumbs"))
                                     .Select(x => Path.GetFileName(x.ToString()));
        
                //Init ProductVM

            }
            //Return view with model
            return View("ProductDetails", model);
        }
    }
}