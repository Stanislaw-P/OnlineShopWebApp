using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

		public async Task<IActionResult> Index()
		{
			var users = await usersManager.Users.ToListAsync();
			return View(_mapper.Map<List<UserViewModel>>(users));
		}

		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> AddAsync(Register register)
		{
			if (register.Email == register.Password)
				ModelState.AddModelError("", "Почта и пароль не должны совпадать!");
			if (await usersManager.FindByEmailAsync(register.Email) != null)
				ModelState.AddModelError("", "Пользователь с такой почтой уже сущестует!");
			if (ModelState.IsValid)
			{
				var newUser = new User { UserName = register.Name, UserSurname = register.Surname, Email = register.Email, PasswordHash = register.Password, PhoneNumber = register.Phone };
				var result = await usersManager.CreateAsync(newUser, register.Password);
				if (result.Succeeded)
				{
					tryAssignUserRoleAsync(newUser);
					return RedirectToAction(nameof(Index));
				}
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

		public async Task<IActionResult> DetailsAsync(Guid id)
		{
			var existingUser = await usersManager.FindByIdAsync(id.ToString());
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
		public async Task<IActionResult> ChangePasswordAsync(NewUserPassword newUserPassword)
		{
			var existingUser = await usersManager.FindByIdAsync(newUserPassword.Id.ToString());
			if (existingUser?.Email == newUserPassword.Password)
				ModelState.AddModelError("", "Почта (логин) и пароль не должны совпадать!");

			if (!ModelState.IsValid)
				return View(newUserPassword);

			var userHashPassword = usersManager.PasswordHasher.HashPassword(existingUser, newUserPassword.Password);
			existingUser.PasswordHash = userHashPassword;
			var result = await usersManager.UpdateAsync(existingUser);

			if (result.Succeeded)
				return RedirectToAction(nameof(Index));

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
			return View(newUserPassword);
		}

		public async Task<IActionResult> EditAsync(Guid id)
		{
			var userAccount = await usersManager.FindByIdAsync(id.ToString());
			return View(_mapper.Map<EditUserViewModel>(userAccount));
		}

		[HttpPost]
		public async Task<IActionResult> EditAsync(EditUserViewModel editUser)
		{
			var existingUser = await usersManager.FindByIdAsync(editUser.Id.ToString());
			if (!ModelState.IsValid)
				return View(editUser);
			if (existingUser == null)
				return NotFound();

			// Не очень как-то обновлять в методе
			existingUser.UserName = editUser.UserName;
			existingUser.UserSurname = editUser.UserSurname;
			existingUser.PhoneNumber = editUser.PhoneNumber;

			var result = await usersManager.UpdateAsync(existingUser);
			if (result.Succeeded)
				return RedirectToAction(nameof(Index));

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
			return View(editUser);
		}

		public async Task<IActionResult> EditRightsAsync(Guid id)
		{
			var user = await usersManager.FindByIdAsync(id.ToString());
			var userRoles = await usersManager.GetRolesAsync(user);
			var roles = await rolesManager.Roles.ToListAsync();

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
		public async Task<IActionResult> EditRightAsync(Guid id, Dictionary<string, bool> userRolesViewModel)
		{
			var selectedRoles = userRolesViewModel.Select(x => x.Key);
			var user = await usersManager.FindByIdAsync(id.ToString());
			if (user == null)
				return NotFound($"Пользователя с id:{id} не существует");
			var userRoles = await usersManager.GetRolesAsync(user);

			// Удаляем все роли и пользователя
			await usersManager.RemoveFromRolesAsync(user, userRoles);
			// Добавляем выбранные роли
			await usersManager.AddToRolesAsync(user, selectedRoles);

			return RedirectToAction(nameof(DetailsAsync), new { Id = id });
		}

		public async Task<IActionResult> RemoveAsync(Guid id)
		{
			var userForRemove = await usersManager.FindByIdAsync(id.ToString());
			if (userForRemove == null)
				return NotFound($"Пользователя с id:{id} не существует");

			var result = await usersManager.DeleteAsync(userForRemove);
			if (result.Succeeded)
				return RedirectToAction(nameof(Index));
			return BadRequest(result.Errors.Select(error => error.Description));
		}

		private async Task tryAssignUserRoleAsync(User user)
		{
			try
			{
				await usersManager.AddToRoleAsync(user, Constants.UserRoleName);
			}
			catch
			{
				// TODO: Тут должно быть логирование исключения
			}
		}
	}
}
