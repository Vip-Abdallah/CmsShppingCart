using CmsShppingCart.Models.Cart;
using CmsShppingCart.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShppingCart.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            //Init Cart List
            var cart = Session["Cart"] as List<CartVM> ?? new List<CartVM>();
            //Check if cart is empty
            if (cart.Count == 0 || Session["Cart"] == null)
            {
                ViewBag.Message = "Your Cart is Empty";
                return View();
            }
            //calculate total and save to viewbag
            decimal Total = 0m;
            foreach (var item in cart)
            {
                Total += item.Total;
            }

            ViewBag.GrandTotal = Total;


            //return view with model
            return View(cart);
        }

        public ActionResult CartPartial()
        {
            //Init CartVM
            CartVM model = new CartVM();

            //Init Quantity
            int qty = 0;
            //Init Price
            decimal price = 0m;
            //Check for cart session
            if (Session["Cart"] != null)
            {
                //Get Total qty and price
                var list = (List<CartVM>)Session["Cart"];
                foreach (var item in list)
                {
                    qty += item.Quantity;
                    price += item.Quantity * item.Price;
                }


            }
            //get Or set qty and price to a
            model.Quantity = qty;
            model.Price = price;

            //Return partial view with model
            return PartialView(model);
        }

        [HttpGet]
        //GET Cart/AddToCartPartial/id
        public ActionResult AddToCartPartial(int id)
        {
            //Init CartVM List
            List<CartVM> cart = Session["Cart"] as List<CartVM> ?? new List<CartVM>();
            //Init CartVM
            CartVM model = new CartVM();
            using (Db db=new Db())
            {
                //get the product
                ProductDTO oProd = db.Products.Find(id);
                //Check if the product is already in cart
                var ProductCart = cart.FirstOrDefault(x => x.ProductID == id);
                //if not, add new
                if (ProductCart == null)
                {
                    cart.Add(new CartVM() {
                        ProductID = id,
                        ProductName = oProd.Name,
                        Quantity = 1,
                        Price = oProd.Price,
                       Image = oProd.ImageName                
                    });
                }
                else
                {
                    //if it is ,increment
                    ProductCart.Quantity++;
                }
            }
            //get total qty and price and add to model
            int qty = 0;
            decimal price = 0m;

            foreach (var item in cart)
            {
                qty += item.Quantity;
                price += item.Quantity * item.Price;
            }
            model.Quantity = qty;
            model.Price = price;

            //save cart back to session
            Session["Cart"] = cart;
            //return partial view with model
            return PartialView(model);
        }

        //GET /Cart/IncrementProduct
        [HttpGet]
        public ActionResult IncrementProduct(int ProductID)
        {
            //Init Cart LIst
            List<CartVM> cart = Session["Cart"] as List<CartVM>;

            //Get CartVM From List
            CartVM model = cart.FirstOrDefault(x => x.ProductID == ProductID);

            //Incremnt qty
            model.Quantity++;
            //Store needed data
            Session["Cart"] = cart;

            //return json with data
            var Result = new { qty = model.Quantity, price = model.Price };
            return Json(Result,JsonRequestBehavior.AllowGet);

        }

        //GET /Cart/DecrementProduct
        [HttpGet]
        public ActionResult DecrementProduct(int ProductID)
        {
            //Init Cart LIst
            List<CartVM> cart = Session["Cart"] as List<CartVM>;

            //Get CartVM From List
            CartVM model = cart.FirstOrDefault(x => x.ProductID == ProductID);

            //Incremnt qty
            model.Quantity--;
            if (model.Quantity<=0)
            {
                cart.Remove(model);
            }

            //Store needed data
            Session["Cart"] = cart;

            //return json with data
            var Result = new { qty = model.Quantity, price = model.Price };
            return Json(Result, JsonRequestBehavior.AllowGet);

        }

        //GET /Cart/DecrementProduct
        [HttpGet]
        public void RemoveProduct(int ProductID)
        {
            //Init Cart LIst
            List<CartVM> cart = Session["Cart"] as List<CartVM>;

            //Get CartVM From List
            CartVM model = cart.FirstOrDefault(x => x.ProductID == ProductID);

            //Remove Product From List
            if (model != null)
            {
                cart.Remove(model);
            }

            //Store needed data
            Session["Cart"] = cart;

        }

        public ActionResult PaypalPartial()
        {
            //Init Cart LIst
            List<CartVM> cart = Session["Cart"] as List<CartVM>;


            return PartialView(cart);
        }
    }
}