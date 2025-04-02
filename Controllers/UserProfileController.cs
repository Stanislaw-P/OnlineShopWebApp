using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
	public class UserProfileController : Controller
	{
		readonly UserManager<User> usersManager;
		readonly IOrdersRepository ordersRepository;
		readonly IMapper _mapper;
		public UserProfileController(UserManager<User> usersManager, IOrdersRepository ordersRepository, IMapper mapper)
		{
			this.usersManager = usersManager;
			this.ordersRepository = ordersRepository;
			_mapper = mapper;
		}

		public IActionResult Index()
		{
			var currentUser = usersManager.GetUserAsync(HttpContext.User).Result;
			return View(_mapper.Map<UserViewModel>(currentUser));
		}

		public IActionResult Order()
		{
			var currentUser = usersManager.GetUserAsync(HttpContext.User).Result;
			var currentUserId = Guid.Parse(currentUser.Id);
			var ordersCurrentUser = ordersRepository.TryGetUserOrders(currentUserId);

			if (ordersCurrentUser == null)
				return NotFound();

			var ordersViewModels = _mapper.Map<List<OrderViewModel>>(ordersCurrentUser);
			return View(ordersViewModels);
		}

		public IActionResult OrderDetails(Guid orderId)
		{
			var existingOrder = ordersRepository.TryGetById(orderId);
			if (existingOrder == null)
				return NotFound();
			return View(_mapper.Map<OrderViewModel>(existingOrder));
		}
	}
}
