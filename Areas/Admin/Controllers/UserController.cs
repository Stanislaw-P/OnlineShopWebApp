using AutoMapper;
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
		readonly RoleManager<IdentityRole> rolesManager;
		readonly IMapper _mapper;

		public UserController(UserManager<User> usersManager, RoleManager<IdentityRole> rolesManager, IMapper mapper)
		{
			this.usersManager = usersManager;
			this.rolesManager = rolesManager;
			_mapper = mapper;
		}

		public IActionResult Index()
		{
			var users = usersManager.Users.ToList();
			return View(_mapper.Map<List<UserViewModel>>(users));
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
				var newUser = new User { UserName = register.Name, UserSurname = register.Surname, Email = register.Email, PasswordHash = register.Password, PhoneNumber = register.Phone };
				var result = usersManager.CreateAsync(newUser, register.Password).Result;
				if (result.Succeeded)
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

		public IActionResult Details(Guid id)
		{
			var existingUser = usersManager.FindByIdAsync(id.ToString()).Result;
			if (existingUser == null)
				return NotFound();
			return View(_mapper.Map<UserViewModel>(existingUser));
		}

		public IActionResult ChangePassword(Guid id)
		{
			NewUserPassword newUserPassword = new NewUserPassword() { Id = id };
			return View(newUserPassword);
		}

		[HttpPost]
		public IActionResult ChangePassword(NewUserPassword newUserPassword)
		{
			var existingUser = usersManager.FindByIdAsync(newUserPassword.Id.ToString()).Result;
			if (existingUser?.Email == newUserPassword.Password)
				ModelState.AddModelError("", "Почта (логин) и пароль не должны совпадать!");
			if (!ModelState.IsValid)
				return View(newUserPassword);

			var userHashPassword = usersManager.PasswordHasher.HashPassword(existingUser, newUserPassword.Password);
			existingUser.PasswordHash = userHashPassword;
			var result = usersManager.UpdateAsync(existingUser).Result;

			if(result.Succeeded)
				return RedirectToAction(nameof(Index));

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
			return View(newUserPassword);
		}

		public IActionResult Edit(Guid id)
		{
			var userAccount = usersManager.FindByIdAsync(id.ToString()).Result;
			return View(_mapper.Map<EditUserViewModel>(userAccount));
		}

		[HttpPost]
		public IActionResult Edit(EditUserViewModel editUser)
		{
			var existingUser = usersManager.FindByIdAsync(editUser.Id.ToString()).Result;
			if (!ModelState.IsValid)
				return View(editUser);
			if (existingUser == null)
				return NotFound();

			// Не очень как-то обновлять в методе
			existingUser.UserName = editUser.UserName;
			existingUser.UserSurname = editUser.UserSurname;
			existingUser.PhoneNumber = editUser.PhoneNumber;

			var result = usersManager.UpdateAsync(existingUser).Result;
			if (result.Succeeded)
				return RedirectToAction(nameof(Index));

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
			return View(editUser);
		}

		public IActionResult EditRights(Guid id)
		{
			var user = usersManager.FindByIdAsync(id.ToString()).Result;
			var userRoles = usersManager.GetRolesAsync(user).Result;
			var roles = rolesManager.Roles.ToList();

			EditRightsViewModel editRightsViewModel = new EditRightsViewModel
			{
				UserName = user.UserName,
				Email = user.Email,
				UserRoles = userRoles.Select(role => new RoleViewModel { Name = role }).ToList(),
				AllRoles = roles.Select(role => new RoleViewModel { Name = role.Name }).ToList()
			};
			return View(editRightsViewModel);
		}

		[HttpPost]
		public IActionResult EditRights(Guid id, Dictionary<string, bool> userRolesViewModel)
		{
			var selectedRoles = userRolesViewModel.Select(x => x.Key);
			var user = usersManager.FindByIdAsync(id.ToString()).Result;
			var userRoles = usersManager.GetRolesAsync(user).Result;
			// Удаляем все роли и пользователя
			usersManager.RemoveFromRolesAsync(user, userRoles).Wait();
			// Добавляем выбранные роли
			usersManager.AddToRolesAsync(user, selectedRoles).Wait();

			return RedirectToAction(nameof(Details), new { Id = id });
			//return Redirect($"/Admin/User/Details?id={id}"); // Ужасный костыль!
		}

		public IActionResult Remove(Guid id)
		{
			var userForRemove = usersManager.FindByIdAsync(id.ToString()).Result;
			var result = usersManager.DeleteAsync(userForRemove).Result;
			if(result.Succeeded)
				return RedirectToAction(nameof(Index));
			return BadRequest(result.Errors.Select(error => error.Description));
		}
	}
}
