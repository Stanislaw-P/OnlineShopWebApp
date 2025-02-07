using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
	public class AccountController : Controller
	{
		readonly IUsersManager usersRepository;

		public AccountController(IUsersManager usersRepository)
		{
			this.usersRepository = usersRepository;
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Login(Login login)
		{
			UserAccount? userAccount = usersRepository.TryGetByEmail(login.UserName);
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
			if (register.UserName == register.Password)
				ModelState.AddModelError("", "Имя и пароль не должны совпадать!");
			if (usersRepository.TryGetByEmail(register.UserName) != null)
				ModelState.AddModelError("", "Пользователь с такой почтой уже сущестует!");
			if (ModelState.IsValid)
			{
				UserAccount newUser = new UserAccount(register.UserName, register.Password);
				usersRepository.Add(newUser);
				return RedirectToAction(nameof(Login));
			}
			return View(register);
		}
	}
}
