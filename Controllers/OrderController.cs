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

		public IActionResult Buy()
		{
			Cart existingCart = cartsRepository.TryGetByUserId(Constants.UserId);
			ordersRepository.Add(existingCart);
			cartsRepository.Clear(Constants.UserId);
			return View();
		}
	}
}
