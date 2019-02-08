using CmsShppingCart.Models.Account;
using CmsShppingCart.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CmsShppingCart.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return Redirect("~/Account/Login");
        }
        [HttpGet]
        //GET : Account/Login
        public ActionResult Login()
        {
            //Confirm user is logged in
            string username = User.Identity.Name;

            if (!string.IsNullOrEmpty(username))
            {
                return RedirectToAction("user-profile");
            }

            //return view
            return View();
        }

        [HttpPost]
        //؛POST : Account/Login
        public ActionResult Login(LoginUserVM model)
        {
            //Check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            bool IsValid = false;

            using (Db db=new Db())
            {
                //check if user valid
                if (db.Users.Any(x=>x.UserName.Equals(model.UserName)&&x.Password.Equals(model.Password)))
                {
                    IsValid = true;
                }
            }

            if (!IsValid)
            {
                ModelState.AddModelError("","Invalid username or password");
                return View(model);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                return Redirect(FormsAuthentication.GetRedirectUrl(model.UserName, model.RememberMe));
            }

            //return view
            return View();
        }
        //GET : Account/create-account
        [HttpGet]
        [ActionName("create-account")]
        public ActionResult CreateAccount()
        {
            return View("CreateAccount");
        }

        //POST : Account/create-account
        [HttpPost]
        [ActionName("create-account")]
        public ActionResult CreateAccount(UserVM model)
        {
            //Check model state
            if (!ModelState.IsValid)
            {
                return View("CreateAccount", model);
            }
            //check if password match
            if (!model.Password.Equals(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "Password do not match");
                return View("CreateAccount", model);
            }
            
            using (Db db=new Db())
            {
                //make sure user is unique
                if (db.Users.Any(x => x.UserName.Equals(model.UserName)))
                {
                    ModelState.AddModelError("", $"Username {model.UserName}is taken");
                    model.UserName = "";
                    return View("CreateAccount", model);
                }
                //Create userDTO
                UserDTO oUserDTO = new UserDTO() {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailAddress= model.EmailAddress,
                    UserName = model.UserName,
                    Password = model.Password
                
                };
                //Add the DTO
                db.Users.Add(oUserDTO);
                //Save
                db.SaveChanges();
                //Add to userRoleDTO
                int id = oUserDTO.Id;
                UserRoleDTO oUserRoleDTO = new UserRoleDTO() {
                    UserID = id,
                    RoleId = 2
                };

                db.UserRoles.Add(oUserRoleDTO);
                db.SaveChanges();
            }

            TempData["SM"] = "You are now registerd and can login";
            return Redirect("~/Account/Login");
        }

        //GET : Account/Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/Account/Login");
        }


        public ActionResult UserNavPartial()
        {
            //Get UserName
            string username = User.Identity.Name;

            //Declare model
            UserNavPartialVM model;
            
            using (Db db=new Db())
            {
                //get user
                UserDTO oUserDTo = db.Users.Where(x => x.UserName.Equals(username)).FirstOrDefault();

                //build model
                model = new UserNavPartialVM() {
                    FirstName = oUserDTo.FirstName,
                    LastName = oUserDTo.LastName
                };
            }
            //return partial view with model
            return PartialView(model);
        }

        //GET Account/User-Profile
        [HttpGet]
        [ActionName("User-Profile")]
        public ActionResult UserProfile()
        {
            //get user name
            string username = User.Identity.Name;

            //Declare model
            UserProfileVM model;

            using (Db db=new Db())
            {
                //get user
                UserDTO oUserDTO = db.Users.FirstOrDefault(x => x.UserName.Equals(username));

                //build model
                model = new UserProfileVM(oUserDTO);
            }
            //return view with model
            return View("UserProfile",model);
        }

        //POST Account/User-Profile
        [HttpPost]
        [ActionName("User-Profile")]
        public ActionResult UserProfile(UserProfileVM model)
        {
            //check model state
            if (!ModelState.IsValid)
            {
                return View("UserProfile",model);
            }
            //check password match if need
            if (!string.IsNullOrEmpty(model.Password))
            {
                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("", "Password do not match");
                    return View("UserProfile", model);
                }
            }
            using (Db db = new Db())
            {
                //get username
                string username = User.Identity.Name;
                //make sure username in uniqe
                if (db.Users.Where(x => x.Id != model.Id).Any(x => x.UserName.Equals(model.UserName)))
                {
                    ModelState.AddModelError("", $"Username {model.UserName} alredy exists");
                    model.UserName = "";
                    return View("UserProfile", model);
                }
                //edit DTO
                UserDTO oUserDTO = db.Users.Find(model.Id);
                oUserDTO.FirstName = model.FirstName;
                oUserDTO.LastName = model.LastName;
                oUserDTO.EmailAddress = model.EmailAddress;

                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    oUserDTO.Password = model.Password;
                }
                //Save 
                db.SaveChanges();
            }

            //Set TEmpData Msg
            TempData["SM"] = "You have edited your profile";

            //Redirect
            return Redirect("~/Account/User-Profile");
        }

    }
}