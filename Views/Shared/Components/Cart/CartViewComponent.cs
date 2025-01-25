using Microsoft.AspNetCore.Mvc;
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
			var productCounts = cart?.Amount ?? 0;

			return View("Cart", productCounts);
		}
	}
}
