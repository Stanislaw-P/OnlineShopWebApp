using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Views.Shared.ViewComponents.CartViewComponents
{
	public class CartViewComponent : ViewComponent
	{
		readonly ICartsRepository cartsRepository;

		public CartViewComponent(ICartsRepository cartsRepository)
		{
			this.cartsRepository = cartsRepository;
		}

		public IViewComponentResult Invoke()
		{
			Cart cart = cartsRepository.TryGetByUserId(Constants.UserId);

			CartViewModel cartViewComponent = cart.ToCartViewModel();
		

			var productCounts = cartViewComponent?.Amount ?? 0;

			return View("Cart", productCounts);
		}

		
	}
}
