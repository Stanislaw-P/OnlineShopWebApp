using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;
using System.Diagnostics;

namespace OnlineShopWebApp.Controllers
{
    public class HomeController : Controller
    {
        List<Product> products = new List<Product>() { 
            new Product("Coffe", 250, "Ice-Latte"),
            new Product("Chocolate", 100, "Milka"),
            new Product("Chocolate", 65, "Alpen")
        };

        public string Index()
        {
            string result = "";
            foreach (var product in products)
            {
                result += product + "\n\n";
            }
            return result;
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
    }
}