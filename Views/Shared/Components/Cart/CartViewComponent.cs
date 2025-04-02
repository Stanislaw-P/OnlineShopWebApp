using AutoMapper;
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
		readonly IMapper _mapper;

		public CartViewComponent(ICartsRepository cartsRepository, IMapper mapper)
		{
			this.cartsRepository = cartsRepository;
			_mapper = mapper;
		}

		public IViewComponentResult Invoke()
		{
			Cart cart = cartsRepository.TryGetByUserId(Constants.UserId);

			CartViewModel cartViewComponent = _mapper.Map<CartViewModel>(cart);

			var productCounts = cartViewComponent?.Amount ?? 0;

			return View("Cart", productCounts);
		}
	}
}
