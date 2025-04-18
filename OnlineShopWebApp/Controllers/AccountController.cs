using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
	public class AccountController : Controller
	{
		readonly UserManager<User> _usersManager;
		readonly SignInManager<User> _signInManager;
		readonly EmailService _emailService;

		public AccountController(UserManager<User> usersManager, SignInManager<User> signInManager, EmailService emailService)
		{
			_usersManager = usersManager;
			_signInManager = signInManager;
			_emailService = emailService;
		}

		public IActionResult Login(string? returnUrl)
		{
			return View(new Login { ReturnUrl = returnUrl });
		}

		[HttpPost]
		public IActionResult Login(Login login)
		{
			if (ModelState.IsValid)
			{
				var user = _usersManager.FindByEmailAsync(login.Email).Result;
				if (user != null)
				{
					var result = _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, false).Result;
					if (result.Succeeded)
						return Redirect(login.ReturnUrl ?? "/Home");
					else
						ModelState.AddModelError("", "Неверный логин или пароль!");
				}
				else
				{
					ModelState.AddModelError("", "Неверный логин или пароль!");
				}
			}
			return View(login);
		}

		public IActionResult Register(string? returnUrl)
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
				User user = new User { Email = register.Email, UserName = register.Name, UserSurname = register.Surname, PhoneNumber = register.Phone };
				// Добавляем пользователя
				var result = _usersManager.CreateAsync(user, register.Password).Result;
				if (result.Succeeded)
				{
					// Установка куки
					_signInManager.SignInAsync(user, false);

					tryAssignUserRole(user);

					// Генерация токена для пользователя
					var code = _usersManager.GenerateEmailConfirmationTokenAsync(user).Result;
					var callbackUrl = Url.Action(
						"ConfirmEmail",
						"Account",
						new { userId = user.Id, code = code },
						protocol: HttpContext.Request.Scheme);

					_emailService.SendEmail(register.Email, "Завершение регистрации на Roseto-Shop",
						$"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>Подтвердить</a>"
					);

					TempData["Message"] = "На вашу почту отправлено письмо с подтверждением регистрации. Пожалуйста, проверьте почту.";

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

		private void tryAssignUserRole(User user)
		{
			try
			{
				_usersManager.AddToRoleAsync(user, Constants.UserRoleName).Wait();
			}
			catch
			{
				// TODO: Тут должно быть логирование исключения
			}
		}


		public IActionResult ConfirmEmail(string userId, string code)
		{
			if (userId == null || code == null)
			{
				return View("Error");
			}
			var user = _usersManager.FindByIdAsync(userId).Result;
			if (user == null)
			{
				return View("Error");
			}

			// Подтверждение почты для пользователя
			var result = _usersManager.ConfirmEmailAsync(user, code).Result;
			if (result.Succeeded)
			{
				TempData["Message"] = "Ваш Email успешно подтвержден!";

				return Redirect("/Home");
			}
			else
				return View("Error");
		}

		public IActionResult Logout()
		{
			_signInManager.SignOutAsync().Wait();
			return RedirectToAction(nameof(HomeController.Index), "Home");
		}
	}
}
