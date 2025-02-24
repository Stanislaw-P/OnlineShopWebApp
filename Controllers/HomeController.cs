using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
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
			List<ProductViewModel> productViewModels = new List<ProductViewModel>();
			foreach (var product in productsDb)
			{
				ProductViewModel productViewModel = new ProductViewModel
				{
					Name = product.Name,
					Cost = product.Cost,
					Description = product.Description,
					ImagePath = product.ImagePath
				};
				productViewModels.Add(productViewModel);
			}
			return View(productViewModels);
        }
    }
}