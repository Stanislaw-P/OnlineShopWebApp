using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
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
			Wishlist wishlist = wishlistsRepository.TryGetByUserId(Constants.UserId);
			return View(wishlist);
		}

		public IActionResult Add(Guid productId)
		{
			//ProductViewModel product = productsRepository.TryGetById(productId);
			//wishlistsRepository.Add(product, Constants.UserId);
			return RedirectToAction("Index", "Home");
		}

		public IActionResult RemoveProduct(Guid productId)
		{
			//ProductViewModel product = productsRepository.TryGetById(productId);
			//wishlistsRepository.RemoveProductByUserId(product, Constants.UserId);
			return RedirectToAction("Index");
		}
	}
}
