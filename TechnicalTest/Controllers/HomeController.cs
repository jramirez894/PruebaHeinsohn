using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using TechnicalTest.Models;
using TechnicalTest.Process;

namespace TechnicalTest.Controllers
{
    public class HomeController : Controller
    {
        ProcessProducts processProducts = new ProcessProducts();

        public IActionResult Index()
        {
            List<Products> lstP = new List<Products>();
            List<Products> lstP2 = new List<Products>();
            Products objP = new Products();
            lstP = processProducts.allProducts();

            //for(int i =0; i < lstP.Count; i++)
            //{
            //    if()
            //}    


            return View(lstP);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public Products getProduct(string filter)
        {
            Products product = new Products();
            ProcessProducts process = new ProcessProducts();

            product = process.getProduct(filter);
            if(product == null)
            {
                product = new Products();
            }
            return product;
        }

        [HttpPost]
        public void FinishSale(string json)
        {
            ProcessProducts process = new ProcessProducts();
            process.SaveSale(json);
        }

    }
}
