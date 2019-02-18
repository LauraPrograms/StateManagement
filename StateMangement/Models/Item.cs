using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StateMangement.Models
{
    public class Item
    {
        public string ItemName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public double Subtotal { get; set; }
        public string ItemID { get; set; }


        public Item(string itemName, string description, double price, string itemID)
        {
            ItemName = itemName;
            Description = description;
            Price = price;            
            ItemID = itemID;

        }
        public Item() { }  //need this for the modelState use (or else it will have a no constructor as an error
    }
}