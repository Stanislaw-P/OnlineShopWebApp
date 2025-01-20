using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
    public class CartController : Controller
    {
		readonly IProductsRepository productsRepository;
        readonly ICartsRepository cartsRepository;

		public CartController(IProductsRepository productsRepository, ICartsRepository cartsRepository)
		{
			this.productsRepository = productsRepository;
			this.cartsRepository = cartsRepository;
		}

		public IActionResult Index()
        {
            Cart cart = cartsRepository.TryGetByUserId(Constants.UserId);
            
            return View(cart);
        }
        
        public IActionResult Add(int productId)
        {
            Product product = productsRepository.TryGetById(productId);
			cartsRepository.Add(product, Constants.UserId);
            return RedirectToAction("Index");
        }
    }
}
