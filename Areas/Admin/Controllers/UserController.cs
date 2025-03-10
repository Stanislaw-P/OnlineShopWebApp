using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using OnlineShop.Db;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Areas.Admin.Models;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
	[Area(Constants.AdminRoleName)]
	[Authorize(Roles = Constants.AdminRoleName)]
	public class UserController : Controller
	{
		readonly UserManager<User> usersManager;

		public UserController(UserManager<User> usersManager)
		{
			this.usersManager = usersManager;
		}

		public IActionResult Index()
		{
			var users = usersManager.Users.ToList();
			return View(users.Select(user => user.ToUserViewModel()).ToList());
		}

		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Add(Register register)
		{
			if (register.Email == register.Password)
				ModelState.AddModelError("", "Почта и пароль не должны совпадать!");
			if (usersManager.FindByEmailAsync(register.Email).Result != null)
				ModelState.AddModelError("", "Пользователь с такой почтой уже сущестует!");
			if (ModelState.IsValid)
			{
				var newUser = new User { UserName = register.Name, Email = register.Email, PasswordHash = register.Password, PhoneNumber = register.Phone };
				var result = usersManager.CreateAsync(newUser, register.Password).Result;
				if(result.Succeeded)
					return RedirectToAction(nameof(Index));
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}
			}
			return View(register);
		}

		public IActionResult Details(string email)
		{
			var existingUser = usersManager.FindByEmailAsync(email).Result;
			if (existingUser == null)
				return NotFound();
			return View(existingUser.ToUserViewModel());
		}

		public IActionResult ChangePassword(string email)
		{
			NewUserPassword newUserPassword = new NewUserPassword() { Email = email };
			return View(newUserPassword);
		}

		[HttpPost]
		public IActionResult ChangePassword(NewUserPassword newUserPassword)
		{
			if (newUserPassword.Email == newUserPassword.Password)
				ModelState.AddModelError("", "Почта (логин) и пароль не должны совпадать!");
			if (!ModelState.IsValid)
				return View(newUserPassword);

			var user = usersManager.FindByEmailAsync(newUserPassword.Email).Result;
			var userHashPassword = usersManager.PasswordHasher.HashPassword(user, newUserPassword.Password);
			user.PasswordHash = userHashPassword;
			usersManager.UpdateAsync(user).Wait();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Edit(string email)
		{
			var userAccount = usersManager.FindByEmailAsync(email).Result;
			return View(userAccount.ToUserViewModel());
		}

		[HttpPost]
		public IActionResult Edit(UserViewModel editUser)
		{
			var existingUser = usersManager.FindByEmailAsync(editUser.Email).Result;
			if (existingUser != null && existingUser.Email != editUser.Email)
				ModelState.AddModelError("", "Пользователь с такой почтой уже сущестует!");
			if (!ModelState.IsValid)
				return View(editUser);
			usersManager.UpdateAsync(existingUser).Wait();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Remove(string email)
		{
			var userForRemove = usersManager.FindByEmailAsync(email).Result;
			usersManager.DeleteAsync(userForRemove).Wait();
			return RedirectToAction(nameof(Index));
		}
	}
}
