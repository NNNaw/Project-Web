using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFinal.Models
{
    public class Items
    {
        public tbl_Product product;
        public int numOrder;
        public int quantumExist;
        public Items()
        {

        }
        public Items(tbl_Product p, int numOrder)
        {
            this.product = p;
            this.numOrder = numOrder;
        }
        public tbl_Product getProduct()
        {
            return product;
        }
        public int getNumOrder()
        {
            return numOrder;
        }
        public void setNumOrder(int newNum)
        {
            this.numOrder = newNum;
        }
    }
}