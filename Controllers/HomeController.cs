using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;
using System.Diagnostics;

namespace OnlineShopWebApp.Controllers
{
    public class HomeController : Controller
    {
        readonly IProductsRepository productsRepository;
		public HomeController(IProductsRepository productsRepository)
		{
			this.productsRepository = productsRepository;
		}

		public IActionResult Index()
        {
            var products = productsRepository.GetAll();
            return View(products);
        }
    }
}