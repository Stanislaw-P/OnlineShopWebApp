using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
	public class AccountController : Controller
	{
		readonly IUsersManager usersManager;

		public AccountController(IUsersManager usersRepository)
		{
			this.usersManager = usersRepository;
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Login(Login login)
		{
			UserAccount? userAccount = usersManager.TryGetByEmail(login.Email);
			if (userAccount == null)
				ModelState.AddModelError("", "Пользователя с такой почтой не существует!");
			if (userAccount != null && userAccount.Password != login.Password)
				ModelState.AddModelError("", "Неправильный пароль!");
			if (!ModelState.IsValid)
			{
				return View(login);
			}
			// TODO: Тут нужно будет передавть текущего пользователя.
			return RedirectToAction(nameof(HomeController.Index), "Home");
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Register(Register register)
		{
			if (register.Email == register.Password)
				ModelState.AddModelError("", "Почта и пароль не должны совпадать!");
			if (usersManager.TryGetByEmail(register.Email) != null)
				ModelState.AddModelError("", "Пользователь с такой почтой уже сущестует!");
			if (ModelState.IsValid)
			{
				UserAccount newUser = new UserAccount(register.Email, register.Password, register.Name, register.Surname, register.Phone);
				usersManager.Add(newUser);
				return RedirectToAction(nameof(Login));
			}
			return View(register);
		}
	}
}
