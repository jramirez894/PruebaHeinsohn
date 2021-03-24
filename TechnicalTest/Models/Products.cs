
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalTest.Models
{
    public class Products
    {
        public int pId { get; set; }
        public string pName { get; set; }
        public decimal pCost { get; set; }
        public decimal pPrice { get; set; }
        public int pState { get; set; }
        public int catId { get; set; }
        public string catName { get; set; }
        public int quantity { get; set; }
    }

    public class RequestProducts
    {
        public int pId { get; set; }
        public int catId { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
    }
}
