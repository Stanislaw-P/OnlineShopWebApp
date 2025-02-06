using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
    public class AccountController : Controller
	{
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Login(Login login)
		{
			if(ModelState.IsValid)
				return RedirectToAction("Index", "Home");
			return View(login);
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Register(Register register)
		{
			if (register.UserName == register.Password)
				ModelState.AddModelError("", "Имя и пароль не должны совпадать!");
			if(ModelState.IsValid)
				return RedirectToAction("Login");
			return View(register);
		}
	}
}
