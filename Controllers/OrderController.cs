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
			return View();
		}

		[HttpPost]
		public IActionResult Buy(UserDeliveryInfo user)
		{
			if (!ModelState.IsValid)
				return View("Index", user);

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
