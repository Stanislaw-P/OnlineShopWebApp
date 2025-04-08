using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Db.Models;

namespace OnlineShopWebApp.Views.Shared.Components.Avatar
{
	public class AvatarViewComponent : ViewComponent
	{
		readonly UserManager<User> _userManager;

		public AvatarViewComponent(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		public IViewComponentResult Invoke()
		{
			var currentUserId = _userManager.GetUserId(HttpContext.User);
			var currentUser = _userManager.Users
				.Include(us => us.Avatar)
				.FirstOrDefaultAsync(us => us.Id == currentUserId)
				.Result;
			return View("Avatar", currentUser.Avatar);
		}
	}
}
