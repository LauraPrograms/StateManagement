using StateMangement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StateMangement.Controllers
{
    public class HomeController : Controller
    {
        List<Item> ItemList = new List<Item>() {
           new Item("Hot Chocolate", "Milk, Cocoa, Sugar, Fat", 1.99, "HC"),
           new Item("Latte",  "Milk, Coffee", 1.99, "L"),
           new Item("Coffee",  "Coffee, Water", 1.00, "C"),
           new Item("Tea", "Black Tea", 1.00, "T"),
           new Item("Frozen Lemonade",  "Lemon, Sugar, Ice", 1.99, "FL")
       };

        List<Item> ShoppingCart = new List<Item>();
        double Total = 0;


        public ActionResult Index()
        {
            ViewBag.CurrentUser = (User)Session["CurrentUser"];
            List<User> userList = new List<User>();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "This lab makes me think about MVC";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Howdy Howdy Howdy";

            return View();
        }
        public ActionResult UserRegistration()
        {
            return View();
        }
        public ActionResult Login()
        {

            return View();
        }
        public ActionResult LoginValidation(User loginUser)
        {
            List<User> userList;
            if (Session["List"] == null)
            {
                ViewBag.ErrorMessage = "No users to validate against.";
                return View("Error");
            }

            userList = (List<User>)Session["List"];
            foreach (User person in userList)
            {
                if (person.FirstName == loginUser.FirstName && person.Password == loginUser.Password)
                {
                    Session["CurrentUser"] = person;//person instead of loginUser because person has all the information saved
                    return RedirectToAction("Details");
                }
            }

            ViewBag.ErrorMessage = "User name and/or password did not validate.";
            return View("Error");


        }
        public ActionResult Details(User user)//EVAN LOOK HERE FIRST. After user fills out registration, they are directed to my details page.
        {
            if (Session["CurrentUser"] != null) //If that key (session) exists at all
            {
                user = (User)Session["CurrentUser"]; //This looks for the key "CurrentUser" and reads it back out
                ViewBag.CurrentUser = user;

                return View();
            }
            else
            {
                if (ModelState.IsValid)//this is checking our user.cs model data (validating it) Link
                {
                    List<User> userList;


                    ViewBag.CurrentUser = user;
                    Session["CurrentUser"] = user; //This saves the variable user into the library (session) in key CurrentUser
                    if (Session["List"] != null) //if session list exists at all, do the following
                    {
                        userList = (List<User>)Session["List"];//this reads from the session
                        userList.Add(user);
                        Session["List"] = userList;
                    }
                    else //if list is not in session
                    {
                        userList = new List<User>();
                        userList.Add(user);
                        Session["List"] = userList; //this makes the new session
                    }
                    //userList.Add(user);
                    //Session["List"] = userList;

                    return View();
                }
                else
                {
                    ViewBag.ErrorMessage = "Registration failed. Try again.";
                    return View("Error");
                }
            }
        }
        public ActionResult LogOut()
        {
            Session.Remove("CurrentUser");
            Session.Remove("CustomerTotal");
            Session.Remove("ShoppingCart");
            Session.Remove("TotalItems");

            return View();
        }
        public ActionResult ListItem()
        {
            int TotalItems = 0;
            if (Session["CurrentUser"] != null)
            {
                ViewBag.ItemsList = ItemList;
                ViewBag.CurrentUser = (User)Session["CurrentUser"];
                if (Session["TotalItems"]!=null)
                {
                    TotalItems = (int)Session["TotalItems"];                    
                }               
                ViewBag.TotalItems = TotalItems;
                return View();
            }
            return RedirectToAction("Login");
        }
        public ActionResult AddItem(string itemName, int Amount)
        {
            int TotalItems = 0;
            bool duplicateItem = false;
            if (Session["ShoppingCart"] != null)
            {
                ShoppingCart = (List<Item>)Session["ShoppingCart"];
            }
            foreach (Item item in ItemList)
            {
                if (item.ItemName == itemName)
                {
                    foreach(Item cartItem in ShoppingCart)
                    {
                        if(item.ItemName == cartItem.ItemName)
                        {
                            cartItem.Amount = cartItem.Amount + Amount;
                            duplicateItem = true;

                        }
                        TotalItems = TotalItems + cartItem.Amount;
                    }
                    if (duplicateItem != true)
                    {
                        item.Amount = Amount;
                        ShoppingCart.Add(item);
                        TotalItems = TotalItems + item.Amount;
                    }

                    
                }

            }
           
            Session["TotalItems"] = TotalItems;
            ViewBag.TotalItems = TotalItems;
            Session["ShoppingCart"] = ShoppingCart;
            return RedirectToAction("ListItem");//we changed this from a view because we need to do the action first before view, so viewbag has something.
        }
        public ActionResult ViewCart()
        {
            
            if (Session["ShoppingCart"] == null)
            {
                ViewBag.Message = "This cart is empty";
            }
            else
            {
                
                ShoppingCart = (List<Item>)Session["ShoppingCart"];
                foreach (Item item in ShoppingCart)
                {
                    item.Subtotal = item.Amount *item.Price;
                    ViewBag.Subtotal = item.Subtotal;
                }
                foreach (Item item in ShoppingCart)
                {
                    Total = Total + item.Subtotal;
                }
                ViewBag.ShoppingCart = ShoppingCart;
                Session["CustomerTotal"] = Total;
                ViewBag.Total = Total;
            }

            return View();
        }

        public ActionResult RemoveItem(string itemName, int Amount)
        {
            if (Session["ShoppingCart"] != null)
            {
                ShoppingCart = (List<Item>)Session["ShoppingCart"];
            }
            foreach (Item cartItem in ShoppingCart)
            {
                if (cartItem.ItemName == itemName)
                {
                    cartItem.Amount = cartItem.Amount - Amount;
                    if (cartItem.Amount <= 0)
                    {
                        ShoppingCart.Remove(cartItem);

                        break;  //without this, the code crashes during the foreach loop with the amount =0;
                    }
                }

            }
            if (ShoppingCart.Count() == 0)
            {
                Session.Remove("ShoppingCart");
            }
            else
            {
                Session["ShoppingCart"] = ShoppingCart;
            }
            return RedirectToAction("ViewCart");
        }
    }
}