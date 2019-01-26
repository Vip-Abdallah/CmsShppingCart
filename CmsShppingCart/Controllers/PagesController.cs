using CmsShppingCart.Models.Data;
using CmsShppingCart.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShppingCart.Controllers
{
    public class PagesController : Controller
    {
        // GET: Pages
        public ActionResult Index(string Page = "")
        {
            //Get/Set Page Slug
            if (Page == "")
            {
                Page = "home";
            }
            //Declare model and DTO
            PageVM model;
            PageDTO oDto;

            //Check if page exsist
            using (Db db = new Db())
            {
                if (!db.Pages.Any(x => x.Slug.Equals(Page)))
                {
                    return RedirectToAction("Index", new { Page = "" });
                }
            }
            //Get Page DTO
            using (Db db = new Db())
            {
                oDto = db.Pages.Where(x => x.Slug.Equals(Page)).SingleOrDefault();
            }
            //Set Page Title
            ViewBag.PageTitle = oDto.Title;
            //Check for Sidebar
            if (oDto.HasSideBar==true)
            {
                ViewBag.Sidebar = "Yes";
            }
            else
            {
                ViewBag.Sidebar = "No";
            }
            //init model
            model = new PageVM(oDto);

            //return view with model 
            return View(model);
        }

        public ActionResult PageMenuPartial()
        {
            //Declare a list of PageVM
            List<PageVM> listPagesVM;
            //Get all pages except home 
            using (Db db=new Db())
            {
                listPagesVM = db.Pages.ToArray().OrderBy(x => x.Sorting)
                    .Where(x => x.Slug != "home").Select(x => new PageVM(x)).ToList();
            }
            //return partial view wih list
            return PartialView(listPagesVM);
        }
    }
}