using CmsShppingCart.Models.Data;
using CmsShppingCart.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PagedList;

namespace CmsShppingCart.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop/Categories
        public ActionResult Categories()
        {
            //Declare List of Models
            List<CategoryVM> CategireyVMList;
            using (Db db=new Db())
            {
                //Init the list
                CategireyVMList = db.Categories
                    .ToArray()
                    .OrderBy(x=>x.Sorting)
                    .Select(x => new CategoryVM(x)).ToList();
            }

            //Return view with list
            return View(CategireyVMList);
        }
        [HttpPost]
        // POST: Admin/Shop/ReorderCategories
        public void ReorderCategories(object [] id)
        {
            //Declare Counter
            int count = 1;
            using (Db db=new Db())
            {
                foreach (string item in id)
                {
                    CategoryDTO oCategoryDTO = db.Categories.Find(int.Parse(item.ToString()));
                    if (oCategoryDTO!=null)
                    {
                        oCategoryDTO.Sorting = count;
                        db.SaveChanges();
                        count++;
                    }
                }
            }

        }

        [HttpPost]
        // POST: Admin/Shop/AddNewCategory
        public string AddNewCategory(string catName)
        {
            //declare id
            string Id;

            
            using (Db db = new Db())
            {
                //check that category name is unique
                if (db.Categories.Any(x=>x.Name==catName))
                {
                    return "titletaken";
                }
                //Init DTO
                CategoryDTO oCategoryDTO = new CategoryDTO();

                //Add to DTO
                oCategoryDTO.Name = catName;
                oCategoryDTO.Slug = catName.Replace(" ", "-").ToLower();
                oCategoryDTO.Sorting = 100;

                //Save DTO
                db.Categories.Add(oCategoryDTO);
                db.SaveChanges();

                //Get the id
                Id = oCategoryDTO.Id.ToString();
                
            }

            //Return Id
            return Id;
        }
        // POST: Admin/Shop/DeleteCategory
        public ActionResult DeleteCategory(int id)
        {
            using (Db db = new Db())
            {
                //get page
                CategoryDTO oCategoryDTO = db.Categories.Find(id);
                //remove the page 
                db.Categories.Remove(oCategoryDTO);

                //save 
                db.SaveChanges();
            }

            //redirect
            return RedirectToAction("Categories");
        }

        [HttpPost]
        // POST: Admin/Shop/RenameCategory
        public string RenameCategory(string id , string newCatName)
        {
            //declare id
            string MSG;
            
            using (Db db = new Db())
            {
                int _ID = int.Parse(id);
                //check that category name is unique
                if (db.Categories.Any(x => x.Id!= _ID && x.Name == newCatName))
                {
                    return "titletaken";
                }
                //Init DTO
                CategoryDTO oCategoryDTO = db.Categories.Find(int.Parse(id));

                //Add to DTO
                oCategoryDTO.Name = newCatName;
                oCategoryDTO.Slug = newCatName.Replace(" ", "-").ToLower();

                //Save DTO
                db.SaveChanges();

                //Set Success msg
                MSG = "Done";

            }

            //Return Id
            return MSG;
        }

        [HttpGet]
        //GET: Admin/Shop/AddProduct
        public ActionResult AddProduct()
        {
            //Init model
            ProductVM model = new ProductVM();

            //Add SelectList of Categories to model 
            using (Db db=new Db())
            {
                model.Categories = new SelectList(db.Categories.ToList(),"Id","Name");
            }

            //retern view with model
            return View(model);
        }
        [HttpPost]
        //POST: Admin/Shop/AddProduct
        public ActionResult AddProduct(ProductVM model,HttpPostedFileBase file)
        {
            //Check model state
            if (!ModelState.IsValid)
            {
                using (Db db = new Db())
                {
                    model.Categories = new SelectList(db.Categories.ToList(),"Id","Name");
                    ModelState.AddModelError("", "Please Enter All Required Filed");
                    return View(model);
                }
            }

            //Make Sure Product Name Is Unique
            using (Db db = new Db())
            {
                if (db.Products.Any(x => x.Name == model.Name))
                {
                    model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                    ModelState.AddModelError("", "That Product Name is taken!");
                    return View(model);
                }
            }

            //Declare Id
            int Id;

            using (Db db = new Db())
            {
                //Make Sure Product Name Is Unique
                if (db.Products.Any(x => x.Name == model.Name))
                {
                    return View(model);
                }
                
                //Init and Save ProductDTO
                ProductDTO oProductDTO = new ProductDTO();
                oProductDTO.Name = model.Name;
                oProductDTO.Slug = model.Name.Replace(" ", "-").Trim();
                oProductDTO.Description = model.Description;
                oProductDTO.Price = model.Price;
                oProductDTO.CategoryId = model.CategoryId;

                CategoryDTO oCategoryDTO = db.Categories.FirstOrDefault(x => x.Id == model.CategoryId);
                oProductDTO.CategoryName = oCategoryDTO.Name;

                db.Products.Add(oProductDTO);
                db.SaveChanges();

                //Get Inserted Product Id
                Id = oProductDTO.Id;
                //Set TempData Message
                TempData["SM"] = "You have added a product!";
                
            }
            #region Upload File

            //Create necessary Directory
            var OrginalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads",Server.MapPath(@"\")));

            string PathString1 = Path.Combine(OrginalDirectory.ToString(),"Products");
            string PathString2 = Path.Combine(OrginalDirectory.ToString(), "Products\\"+Id.ToString());
            string PathString3 = Path.Combine(OrginalDirectory.ToString(), "Products\\" + Id.ToString() + "\\Thumbs");
            string PathString4 = Path.Combine(OrginalDirectory.ToString(), "Products\\" + Id.ToString() + "\\Gallery");
            string PathString5 = Path.Combine(OrginalDirectory.ToString(), "Products\\" + Id.ToString() + "\\Gallery\\Thumbs");

            if (!Directory.Exists(PathString1))
                Directory.CreateDirectory(PathString1);

            if (!Directory.Exists(PathString2))
                Directory.CreateDirectory(PathString2);

            if (!Directory.Exists(PathString3))
                Directory.CreateDirectory(PathString3);

            if (!Directory.Exists(PathString4))
                Directory.CreateDirectory(PathString4);

            if (!Directory.Exists(PathString5))
                Directory.CreateDirectory(PathString5);

            //Check if file was uploaded
            if (file != null && file.ContentLength > 0)
            {
                //Get File ext
                string ext = file.ContentType.ToString();
                //Verify ext
                if (ext!="image/jpg" &&
                    ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/gif" &&
                    ext != "image/png")
                {
                    using (Db db = new Db())
                    {
                        model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                        ModelState.AddModelError("", "The Image was not uploaded - wtong image extension!");
                        return View(model);
                    }
                }

                //init image name
                string ImageName = Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);

                //save image name to DTO
                using (Db db=new Db())
                {
                    ProductDTO oProductDTO = db.Products.Find(Id);
                    oProductDTO.ImageName = ImageName;
                    db.SaveChanges();
                }
                //set orginal and thumb image paths
                var Path1 = string.Format("{0}\\{1}",PathString2,ImageName);
                var Path2 = string.Format("{0}\\{1}", PathString3, ImageName);

                //save orginal
                file.SaveAs(Path1);
                //create and save thumb
                WebImage oWebImage = new WebImage(file.InputStream);
                oWebImage.Resize(200, 200);

                oWebImage.Save(Path2);
            }

            #endregion
            //Redirect
            return RedirectToAction("AddProduct");
        }

        //GET: Admin/Shop/Products
        public ActionResult Products(int? Page,int? CatId)
        {
            //Declare a list of ProductVM
            List<ProductVM> ListOfProduct;
            //Set Page Numer
            int PageNumber = Page ?? 1;
            using (Db db=new Db())
            {
                //Init the list
                ListOfProduct = db.Products.ToArray()
                    .Where(x=>CatId==null || CatId==0 || x.CategoryId==CatId).Select(x => new ProductVM(x)).ToList();

                //populate categories select list 
                ViewBag.Categories = new SelectList(db.Categories.ToList(),"Id","Name");
                //set selected category 
                ViewBag.SelectedCategoryID = (CatId??0).ToString();
            }

            //set pegnation
            var OnePageOfProducts = ListOfProduct.ToPagedList(PageNumber, 3);

            ViewBag.OnePageOfProducts = OnePageOfProducts;

            //retern view with a list
            return View(ListOfProduct);
        }

        //GET: Admin/Shop/EditProduct/Id
        [HttpGet]
        public ActionResult EditProduct(int Id)
        {
            //Declare ProductVm
            ProductVM oProductVM;
            using (Db db=new Db())
            {
                //Get Product
                ProductDTO oProductDTO = db.Products.Find(Id);

                //Make Sure Product Exsist
                if (oProductDTO==null)
                {
                    return Content("That Product dose not exist");
                }
                //Init model
                oProductVM = new ProductVM(oProductDTO);

                //Make a select list
                oProductVM.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");

                //Get all gallary image 
                oProductVM.GallaryImage = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + oProductVM.Id+ "/Gallery/Thumbs"))
                                                   .Select(fn=>Path.GetFileName(fn));
            }

            //return view with model
            return View(oProductVM);
        }

        //POST: Admin/Shop/EditProduct
        [HttpPost]
        public ActionResult EditProduct(ProductVM model , HttpPostedFileBase file)
        {
            //Get Product Id
            int Id = model.Id;
            //Publulate categories select list and image gallery
            using (Db db=new Db())
            {
                model.Categories = new SelectList(db.Categories.ToList(),"Id","Name");
            }
            model.GallaryImage = Directory.EnumerateDirectories(Server.MapPath("~/Images/Uploads/Products/" + model.Id + "/Gallery/Thumbs"))
                                           .Select(fn => Path.GetFileName(fn)).ToList();
            //check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //make sure product name is unique
            using (Db db=new Db())
            {
                if (db.Products.Any(x => x.Id != Id && x.Name == model.Name))
                {
                    ModelState.AddModelError("","That Product Name is Taken!");
                    return View(model);
                } 
            }
            //Update product
            using (Db db=new Db())
            {
                ProductDTO oProductDTO = db.Products.Find(Id);
                oProductDTO.Name = model.Name;
                oProductDTO.Slug = model.Name.Replace(" ", "-").Trim();
                oProductDTO.Description = model.Description;
                oProductDTO.Price = model.Price;
                oProductDTO.CategoryId = model.CategoryId;
                oProductDTO.ImageName = model.ImageName;

                CategoryDTO oCategoryDTO = db.Categories.FirstOrDefault(x => x.Id == model.CategoryId);
                oProductDTO.CategoryName = oCategoryDTO.Name;
                
                db.SaveChanges();
            }
            //set TempDate Message 
            TempData["SM"] = "You Have Edited the product!";
            #region Upload Image

            //Check if file was uploaded
            if (file != null && file.ContentLength > 0)
            {
               
                //Get File ext
                string ext = file.ContentType.ToString();
                //Verify ext
                if (ext != "image/jpg" &&
                    ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/gif" &&
                    ext != "image/png")
                {
                    using (Db db = new Db())
                    {
                        model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                        ModelState.AddModelError("", "The Image was not uploaded - wtong image extension!");
                        return View(model);
                    }
                }

                //Set Upload Directory
                var OrginalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
                
                string PathString1 = Path.Combine(OrginalDirectory.ToString(), "Products\\" + Id.ToString());
                string PathString2 = Path.Combine(OrginalDirectory.ToString(), "Products\\" + Id.ToString() + "\\Thumbs");

                DirectoryInfo dir1 = new DirectoryInfo(PathString1);
                DirectoryInfo dir2 = new DirectoryInfo(PathString2);

                foreach (FileInfo oFile in dir1.GetFiles())
                {
                    if (oFile.Name == model.ImageName)
                        oFile.Delete();
                }
                foreach (FileInfo oFile in dir2.GetFiles())
                {
                    if (oFile.Name == model.ImageName)
                        oFile.Delete();
                }
                //init image name
                string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                //save image name to DTO
                using (Db db = new Db())
                {
                    ProductDTO oProductDTO = db.Products.Find(Id);
                    oProductDTO.ImageName = ImageName;
                    db.SaveChanges();
                }
                //set orginal and thumb image paths
                var Path1 = string.Format("{0}\\{1}", PathString2, ImageName);
                var Path2 = string.Format("{0}\\{1}", PathString2, ImageName);

                //save orginal
                file.SaveAs(Path1);
                //create and save thumb
                WebImage oWebImage = new WebImage(file.InputStream);
                oWebImage.Resize(200, 200);

                oWebImage.Save(Path2);

            }

            #endregion

            //Redirect 
            return RedirectToAction("EditProduct");
        }
    }
}