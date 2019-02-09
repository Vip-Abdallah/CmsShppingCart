using CmsShppingCart.Models.Data;
using CmsShppingCart.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShppingCart.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            // Declare List Of PageVM
            List<PageVM> PagesList;
            
            using (Db db=new Db())
            {
                //Init The List
                PagesList = db.Pages.ToArray()
                                  .OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }
            //Return view with list
            return View(PagesList);
        }

        [HttpGet]
        //Get : Admin/Pages/AddPage
        public ActionResult AddPage()
        {
            return View();
        }

        [HttpPost]
        //Post : Admin/Pages/AddPage
        public ActionResult AddPage(PageVM model)
        {
            //check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db=new Db())
            {
                //declare slug
                string Slug;

                //Init pageDTO
                PageDTO oPageDTO = new PageDTO();

                //DTO Title
                oPageDTO.Title = model.Title;

                //check for and set slug if need be
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    Slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    Slug = model.Slug.Replace(" ", "-").ToLower();
                }

                //make sure title and slug are unique
                if (db.Pages.Any(x=>x.Title==model.Title) || db.Pages.Any(x => x.Slug == model.Slug))
                {
                    ModelState.AddModelError("", "That title or slug already exists.");
                    return View(model);
                }

                //DTO the rest
                oPageDTO.Slug = Slug;
                oPageDTO.Body = model.Body;
                oPageDTO.HasSideBar = model.HasSideBar;
                oPageDTO.Sorting = 100;

                //save DTO
                db.Pages.Add(oPageDTO);
                if (db.SaveChanges()==0)
                {
                    ModelState.AddModelError("", "Somthing wrong was happent please try again.");
                    return View(model);
                }
            }

            //set TempData message
            TempData["SM"] = "You have added anew page!";

            //Redirect
            return RedirectToAction("AddPage");
        }

        [HttpGet]
        //Get : Admin/Pages/EditPage/id
        public ActionResult EditPage(int id)
        {
            //Declare PageMV
            PageVM model;
            using (Db db=new Db())
            {

                //Get The Page
                PageDTO oPageDTO = db.Pages.Find(id);

                //Confirm page exists
                if (oPageDTO==null)
                {
                    return Content("The Page Dos not exists");
                }

                //Init PageMV
                model = new PageVM(oPageDTO);

            }
            //Return View with the model
            return View(model);
        }

        [HttpPost]
        //Post : Admin/Pages/EditPage/id
        public ActionResult EditPage(PageVM model)
        {
            //Check model status
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (Db db=new Db())
            {
                //get page id
                int id = model.Id;
                //Init slug
                string slug = "home";
                //get the page
                PageDTO oPageDTO = db.Pages.Find(id);
                //DTO the title
                oPageDTO.Title = model.Title;
                //check for slug and set it if need be
                if (model.Slug!="home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }
                //make sure title and slug are unique
                if (db.Pages.Any(x => x.Id!=id && x.Title == model.Title) ||
                    db.Pages.Any(x => x.Id != id && x.Slug == model.Slug))
                {
                    ModelState.AddModelError("", "That title or slug already exists.");
                    return View(model);
                }

                //DTO the rest
                oPageDTO.Slug = slug;
                oPageDTO.Body = model.Body;
                oPageDTO.HasSideBar = model.HasSideBar;
                

                //save DTO
               
                if (db.SaveChanges() == 0)
                {
                    ModelState.AddModelError("", "Somthing wrong was happent please try again.");
                    return View(model);
                }

            }

            //Set TempData Message
            TempData["SM"] = "you have Edited the page!";
            //Reditect
            return RedirectToAction("EditPage");
        }

        [HttpGet]
        //Get : Admin/Pages/PageDetails/id
        public ActionResult PageDetails(int id)
        {
            //Declate PageMV
            PageVM model;
            
            using (Db db=new Db())
            {
                //Get Page
                PageDTO oPageDTO = db.Pages.Find(id);
                //Confirm Page exists
                if (oPageDTO == null)
                {
                    return Content("The Page Dos not exists");
                }
                //Init PageVM
                model = new PageVM(oPageDTO);

            }
            //Return View With Model
            return View(model);
        }

        public ActionResult DeletePage(int id)
        {
            using (Db db=new Db())
            {
                //get page
                PageDTO oPageDTO = db.Pages.Find(id);
                //remove the page 
                db.Pages.Remove(oPageDTO);

                //save 
                db.SaveChanges();
            }

            //redirect
            return RedirectToAction("Index");
        }

        [HttpPost]
        //Post : Admin/Pages/ReorderPages
        public void ReorderPages(string [] id)
        {

            using (Db db=new Db())
            {
                //set init count
                int count = 1;

                //declate PageDTO
                PageDTO oPageDTO;
                //Set sorting foreatch page 
                foreach (string PageID in id)
                {
                    oPageDTO = db.Pages.Find(int.Parse(PageID));
                    oPageDTO.Sorting = count;
                    db.SaveChanges();
                    count++;
                }
            }
        }

        [HttpGet]
        //Get : Admin/Pages/EditSidebar
        public ActionResult EditSidebar()
        {
            //Declare model
            SidebarVM model;
            using (Db db=new Db())
            {
                //Get the DTO
                SidebarDTO oSidebarDTO = db.Sidebar.Find(1);
                //Init model
                model = new SidebarVM(oSidebarDTO);
            }
            //Return view with model
            return View(model);
        }

        [HttpPost]
        //Post : Admin/Pages/EditSidebar
        public ActionResult EditSidebar(SidebarVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db=new Db())
            {
                //Get The DTO
                SidebarDTO oSidebarDTO = db.Sidebar.Find(1);

                //DTO the Body
                oSidebarDTO.Body = model.Body;

                //Save DTO
                db.SaveChanges();
            }
            //Set TempData message
            TempData["SM"] = "You have edited the sidebar";
            //Redirect
            return RedirectToAction("EditSidebar");
        }
    }
}