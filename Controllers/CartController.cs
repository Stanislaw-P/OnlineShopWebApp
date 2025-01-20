using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
    public class CartController : Controller
    {
		readonly ProductsRepository productRepitory;
        readonly CartsRepository cartsRepository;

		public CartController(ProductsRepository productsRepository, CartsRepository cartsRepository)
		{
			productRepitory = productsRepository;
			this.cartsRepository = cartsRepository;
		}

		public IActionResult Index()
        {
            Cart cart = cartsRepository.TryGetByUserId(Constants.UserId);
            
            return View(cart);
        }
        
        public IActionResult Add(int productId)
        {
            Product product = productRepitory.TryGetById(productId);
			cartsRepository.Add(product, Constants.UserId);
            return RedirectToAction("Index");
        }
    }
}
