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
		public IActionResult Buy(Order order)
		{
			Cart existingCart = cartsRepository.TryGetByUserId(Constants.UserId);
			order.Cart = existingCart;
			ordersRepository.Add(order);
			cartsRepository.ClearCartByUserId(Constants.UserId);
			return View();
		}
	}
}
