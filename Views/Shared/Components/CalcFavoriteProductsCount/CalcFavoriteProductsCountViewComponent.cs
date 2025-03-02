using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;

namespace OnlineShopWebApp.Views.Shared.Components.FavoriteCount
{
	public class CalcFavoriteProductsCountViewComponent : ViewComponent
	{
		readonly IFavoriteRepository favoriteRepository;

		public CalcFavoriteProductsCountViewComponent(IFavoriteRepository favoriteRepository)
		{
			this.favoriteRepository = favoriteRepository;
		}

		public IViewComponentResult Invoke()
		{
			var productsCount = favoriteRepository.GetAll(Constants.UserId).Count;
			return View("FavoriteProductsCountView", productsCount);
		}
	}
}
