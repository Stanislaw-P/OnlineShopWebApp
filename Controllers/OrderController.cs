using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Helpers;
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
		public IActionResult Buy(UserDeliveryInfoViewModel userViewModel)
		{
			if (!ModelState.IsValid)
				return View("Index", userViewModel);

			Cart existingCartDb = cartsRepository.TryGetByUserId(Constants.UserId);
			Order order = new Order
			{
				// Тут наверно не хватает других свойств
				User = userViewModel.ToUser(),
				Items = existingCartDb.Items
			};

			ordersRepository.Add(order);

			cartsRepository.ClearCartByUserId(Constants.UserId);
			return View();
		}
	}
}
