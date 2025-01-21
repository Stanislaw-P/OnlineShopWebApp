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

        public IActionResult Decrease(int productId)
        {
            Cart currentCart = cartsRepository.TryGetByUserId(Constants.UserId);
            currentCart.Decreace(productId);
            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            Cart currentCart = cartsRepository.TryGetByUserId(Constants.UserId);
            currentCart.Clear();
            return RedirectToAction("Index");
		}
	}
}
