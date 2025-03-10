using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
	public class AccountController : Controller
	{
		readonly IUsersManager usersManager;
		readonly UserManager<User> _usersManager;
		readonly SignInManager<User> _signInManager;

		public AccountController(IUsersManager usersRepository, UserManager<User> usersManager, SignInManager<User> signInManager)
		{
			this.usersManager = usersRepository;
			_usersManager = usersManager;
			_signInManager = signInManager;
		}

		public IActionResult Login(string returnUrl)
		{
			return View(new Login { ReturnUrl = returnUrl });
		}

		[HttpPost]
		public IActionResult Login(Login login)
		{
			if (ModelState.IsValid)
			{
				var result = _signInManager.PasswordSignInAsync(login.Email, login.Password, login.RememberMe, false).Result;
				if (result.Succeeded)
					return Redirect(login.ReturnUrl ?? "/Home");
				else
					ModelState.AddModelError("", "Неверный логин или пароль!");
			}
			return View(login);
		}

		public IActionResult Register(string returnUrl)
		{
			return View(new Register { ReturnUrl = returnUrl });
		}

		[HttpPost]
		public IActionResult Register(Register register)
		{
			if (register.Email == register.Password)
				ModelState.AddModelError("", "Почта и пароль не должны совпадать!");
			if (_usersManager.FindByEmailAsync(register.Email).Result != null)
				ModelState.AddModelError("", "Пользователь с такой почтой уже сущестует!");
			if (ModelState.IsValid)
			{
				User user = new User { Email = register.Email, UserName = register.Name };
				// Добавляем пользователя
				var result = _usersManager.CreateAsync(user, register.Password).Result;
				if (result.Succeeded)
				{
					// Установка куки
					_signInManager.SignInAsync(user, false);
					return Redirect(register.ReturnUrl ?? "/Home");
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}
			return View(register);
		}

		public IActionResult Logout()
		{
			_signInManager.SignOutAsync().Wait();
			return RedirectToAction(nameof(HomeController.Index), "Home");
		}
	}
}
