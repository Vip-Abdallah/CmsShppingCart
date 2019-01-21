using CmsShppingCart.Models.Data;
using CmsShppingCart.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}