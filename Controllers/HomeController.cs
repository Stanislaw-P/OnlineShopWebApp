using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShopWebApp.Helpers;
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
            var productsDb = productsRepository.GetAll();
			return View(productsDb.ToProductViewModels());
        }
    }
}