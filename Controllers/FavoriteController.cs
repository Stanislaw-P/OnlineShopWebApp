using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShopWebApp.Helpers;

namespace OnlineShopWebApp.Controllers
{
	[Authorize]
    public class FavoriteController : Controller
	{
		readonly IFavoriteRepository favoritesRepository;
		readonly IProductsRepository productsRepository;

		public FavoriteController(IFavoriteRepository favoritesRepository, IProductsRepository productsRepository)
		{
			this.favoritesRepository = favoritesRepository;
			this.productsRepository = productsRepository;
		}

		public IActionResult Index()
		{
			var products = favoritesRepository.GetAll(Constants.UserId);
			return View(products.ToProductViewModels());
		}

		public IActionResult Add(Guid productId)
		{
			var product = productsRepository.TryGetById(productId);
			favoritesRepository.Add(product, Constants.UserId);
			return RedirectToAction(nameof(Index), "Home");
		}

		public IActionResult RemoveProduct(Guid productId)
		{
			var product = productsRepository.TryGetById(productId);
			favoritesRepository.Remove(product, Constants.UserId);
			return RedirectToAction(nameof(Index));
		}
	}
}
