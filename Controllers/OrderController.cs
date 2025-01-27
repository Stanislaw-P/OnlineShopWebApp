using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
	public class OrderController : Controller
	{
		readonly ICartsRepository cartsRepository;
		readonly IOrdersRepository ordersRepository;

		public OrderController(ICartsRepository cartsRepository, IOrdersRepository ordersRepository)
		{
			this.cartsRepository = cartsRepository;
			this.ordersRepository = ordersRepository;
		}

		public IActionResult Index()
		{
			Cart cart = cartsRepository.TryGetByUserId(Constants.UserId);
			return View(cart);
		}

		[HttpPost]
		public IActionResult Buy(UserDeliveryInfo user)
		{
			Cart existingCart = cartsRepository.TryGetByUserId(Constants.UserId);
			Order order = new Order
			{
				User = user,
				Items = existingCart.Items
			};
			ordersRepository.Add(order);

			cartsRepository.ClearCartByUserId(Constants.UserId);
			return View();
		}
	}
}
