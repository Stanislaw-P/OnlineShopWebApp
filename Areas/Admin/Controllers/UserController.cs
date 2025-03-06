using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using OnlineShop.Db;
using OnlineShopWebApp.Areas.Admin.Models;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
	[Area(Constants.AdminRoleName)]
	[Authorize(Roles = Constants.AdminRoleName)]
	public class UserController : Controller
	{
		readonly IUsersManager usersManager;

		public UserController(IUsersManager usersManager)
		{
			this.usersManager = usersManager;
		}

		public IActionResult Index()
		{
			List<UserAccount> users = usersManager.GetAll();
			return View(users);
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
			if (usersManager.TryGetByEmail(register.Email) != null)
				ModelState.AddModelError("", "Пользователь с такой почтой уже сущестует!");
			if (ModelState.IsValid)
			{
				UserAccount newUser = new UserAccount(register.Email, register.Password, register.Name, register.Surname, register.Phone);
				usersManager.Add(newUser);
				return RedirectToAction(nameof(Index));
			}
			return View(register);
		}

		public IActionResult Details(Guid id)
		{
			UserAccount? existingUser = usersManager.TryGetById(id);
			if (existingUser == null)
				return NotFound();
			return View(existingUser);
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
			usersManager.ChangePassword(newUserPassword);
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Edit(Guid id)
		{
			UserAccount userAccount = usersManager?.TryGetById(id);
			return View(userAccount);
		}

		[HttpPost]
		public IActionResult Edit(UserAccount editUser)
		{
			var existingUser = usersManager.TryGetByEmail(editUser.Email);
			if (existingUser != null && existingUser.Id != editUser.Id)
				ModelState.AddModelError("", "Пользователь с такой почтой уже сущестует!");
			if (!ModelState.IsValid)
				return View(editUser);
			usersManager.EditUser(editUser);
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Remove(Guid id)
		{
			usersManager.RemoveById(id);
			return RedirectToAction(nameof(Index));
		}
	}
}
