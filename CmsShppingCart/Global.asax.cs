using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CmsShppingCart.Models.Data;
using System.Security.Principal;

namespace CmsShppingCart
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest()
        {
            //Check If User is logged in
            if (User == null)
            {
                return;
            }
            //Get Username
            string username = User.Identity.Name;
            //Declare array of roles
            string[] roles = null;

            using (Db db=new Db())
            {
                //Populate roles
                UserDTO oUserDto = db.Users.FirstOrDefault(x => x.UserName.Equals(username));

                roles = db.UserRoles.Where(x => x.UserID == oUserDto.Id)
                    .Select(x => x.Role.Name).ToArray();
            }

            //build IPrencipal object
            IIdentity UserIdentity = new GenericIdentity(username);
            IPrincipal newUserObj = new GenericPrincipal(UserIdentity, roles);

            //Update context.user
            Context.User = newUserObj;
        }
    }
}
