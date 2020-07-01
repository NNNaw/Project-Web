using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFinal.Models
{
    public class Cart
    {
        public Dictionary<int, Items> ListCarts;
        public Cart()
        {
            ListCarts = new Dictionary<int, Items>();
        }
        public Dictionary<int, Items> getListCart()
        {
            return ListCarts;
        }
      
        public void setListCart(Dictionary<int, Items> listNew)
        {
            this.ListCarts = listNew;
        }
        public void AddToCart(int key,Items item,int newNum)
        {
            bool isValid = ListCarts.ContainsKey(key);
            if (isValid)
            {
                int oldNum = item.getNumOrder();
                item.setNumOrder(oldNum + newNum);
                ListCarts[key] = item;
            }
            else
            {
                ListCarts[key] = item;
            }
        }
        
        public void DeleteItem(int key)
        {
            bool isValid = ListCarts.ContainsKey(key);
            if (isValid)
            {
                ListCarts.Remove(key);
            }
        }
        public int CountItems()
        {
            return ListCarts.Count();
        } 
    }
}