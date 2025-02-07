using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
    public class AccountController : Controller
	{
		readonly IUsersRepository usersRepository;

		public AccountController(IUsersRepository usersRepository)
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
			User? existingUser = usersRepository.TryGetByEmail(login.UserName);
			if (existingUser == null)
				ModelState.AddModelError("", "Пользователя с такой почтой не существует!");
			if (!usersRepository.PasswordIsCorrect(existingUser, login.Password))
				ModelState.AddModelError("", "Неправильный пароль!");
			if (ModelState.IsValid)
			{
				// TODO: Тут нужно будет передавть текущего пользователя.
				return RedirectToAction("Index", "Home");
			}
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
			if (usersRepository.TryGetByEmail(register.UserName) != null)
				ModelState.AddModelError("", "Пользователь с такой почтой уже сущестует!");
			if (ModelState.IsValid)
			{
				User newUser = new User(register.UserName, register.Password);
				usersRepository.Add(newUser);
				return RedirectToAction("Login");
			}
			return View(register);
		}
	}
}
