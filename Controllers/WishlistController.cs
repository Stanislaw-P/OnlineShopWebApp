using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
	public class WishlistController : Controller
	{
		readonly IWishlistsRepository wishlistsRepository;
		readonly IProductsRepository productsRepository;

		public WishlistController(IWishlistsRepository wishlistsRepository, IProductsRepository productsRepository)
		{
			this.wishlistsRepository = wishlistsRepository;
			this.productsRepository = productsRepository;
		}

		public IActionResult Index()
		{
			Wishlist? wishlist = wishlistsRepository.TryGetByUserId(Constants.UserId);
			return View(wishlist);
		}

		public IActionResult Add(int productId)
		{
			Product product = productsRepository.TryGetById(productId);
			wishlistsRepository.Add(product, Constants.UserId);
			return RedirectToAction("Index");
		}

		public IActionResult RevomeProduct(int productId)
		{
			Product product = productsRepository.TryGetById(productId);
			wishlistsRepository.RemoveProductByUserId(product, Constants.UserId);
			return RedirectToAction("Index");
		}
	}
}
