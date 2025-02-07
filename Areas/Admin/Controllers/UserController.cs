﻿using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Areas.Admin.Models;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
	[Area("Admin")]
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
	}
}
