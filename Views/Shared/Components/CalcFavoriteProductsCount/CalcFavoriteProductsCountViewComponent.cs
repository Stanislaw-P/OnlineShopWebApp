using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShop.Db.Models;

namespace OnlineShopWebApp.Views.Shared.Components.FavoriteCount
{
	public class CalcFavoriteProductsCountViewComponent : ViewComponent
	{
		readonly IFavoriteRepository favoriteRepository;
		readonly UserManager<User> _userManager;

		public CalcFavoriteProductsCountViewComponent(IFavoriteRepository favoriteRepository, UserManager<User> userManager)
		{
			this.favoriteRepository = favoriteRepository;
			_userManager = userManager;
		}

		public IViewComponentResult Invoke()
		{
			var currentUserId = _userManager.GetUserId(HttpContext.User);
			var productsCount = favoriteRepository.GetAll(currentUserId).Count;
			return View("FavoriteProductsCountView", productsCount);
		}
	}
}
