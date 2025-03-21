using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
	[Authorize]
	public class OrderController : Controller
	{
		readonly ICartsRepository cartsRepository;
		readonly IOrdersRepository ordersRepository;
		readonly UserManager<User> userManager;

		public OrderController(ICartsRepository cartsRepository, IOrdersRepository ordersRepository, UserManager<User> userManager)
		{
			this.cartsRepository = cartsRepository;
			this.ordersRepository = ordersRepository;
			this.userManager = userManager;
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

			var currentUser = userManager.GetUserAsync(HttpContext.User).Result;
			Guid idCurrentUser = Guid.Parse(currentUser.Id);

			Cart existingCartDb = cartsRepository.TryGetByUserId(Constants.UserId); // TODO: тут нужно поменять на текущего пользователя
			Order order = new Order
			{
				// Тут наверно не хватает других свойств. Не знаю под чем я этот бред писал...
				User = userViewModel.ToUserDeliveryInfo(idCurrentUser),
				Items = existingCartDb.Items
			};

			ordersRepository.Add(order);

			cartsRepository.ClearCartByUserId(Constants.UserId);
			return View();
		}
	}
}
