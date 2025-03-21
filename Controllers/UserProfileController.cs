using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Helpers;

namespace OnlineShopWebApp.Controllers
{
    public class UserProfileController : Controller
    {
        readonly UserManager<User> usersManager;
        readonly IOrdersRepository ordersRepository;

		public UserProfileController(UserManager<User> usersManager, IOrdersRepository ordersRepository)
		{
			this.usersManager = usersManager;
			this.ordersRepository = ordersRepository;
		}

		public IActionResult Index()
        {
            var currentUser = usersManager.GetUserAsync(HttpContext.User).Result;
            return View(currentUser.ToUserViewModel());
        }

        public IActionResult Order()
        {
			var currentUser = usersManager.GetUserAsync(HttpContext.User).Result;
            var currentUserId = Guid.Parse(currentUser.Id);
            var ordersCurrentUser = ordersRepository.TryGetUserOrders(currentUserId);
            if (ordersCurrentUser == null)
                return NotFound();
            return View(ordersCurrentUser.ToOrdersViewModel());
		}

        public IActionResult OrderDetails(Guid orderId)
        {
            var existingOrder = ordersRepository.TryGetById(orderId);
            if (existingOrder == null)
                return NotFound();
            return View(existingOrder.ToOrderViewModel());
        }
	}
}
