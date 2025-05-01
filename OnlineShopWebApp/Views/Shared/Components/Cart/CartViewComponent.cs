using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
		readonly UserManager<User> _userManager;
		readonly IMapper _mapper;

		public CartViewComponent(ICartsRepository cartsRepository, IMapper mapper, UserManager<User> userManager)
		{
			this.cartsRepository = cartsRepository;
			_mapper = mapper;
			_userManager = userManager;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var currentUserId = _userManager.GetUserId(HttpContext.User);

			Cart? cart = await cartsRepository.TryGetByUserIdAsync(currentUserId);

			CartViewModel cartViewComponent = _mapper.Map<CartViewModel>(cart);

			var productCounts = cartViewComponent?.Amount ?? 0;

			return View("Cart", productCounts);
		}
	}
}
