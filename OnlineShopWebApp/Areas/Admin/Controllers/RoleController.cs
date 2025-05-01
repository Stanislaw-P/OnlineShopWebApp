using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShopWebApp.Areas.Admin.Models;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
	[Area(Constants.AdminRoleName)]
	[Authorize(Roles = Constants.AdminRoleName)]
	public class RoleController : Controller
	{
		readonly RoleManager<IdentityRole> rolesManager;
		readonly IMapper _mapper;

		public RoleController(RoleManager<IdentityRole> rolesManager, IMapper mapper)
		{
			this.rolesManager = rolesManager;
			_mapper = mapper;
		}

		public IActionResult Index()
		{
			var roles = rolesManager.Roles.ToList();
			return View(_mapper.Map<List<RoleViewModel>>(roles));
		}

		public async Task<IActionResult> RemoveAsync(string roleName)
		{
			var existingRole = await rolesManager.FindByNameAsync(roleName);
			if (existingRole != null)
				await rolesManager.DeleteAsync(existingRole);
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> AddAsync(RoleViewModel role)
		{
			if (await rolesManager.FindByNameAsync(role.Name) != null)
				ModelState.AddModelError("", "Такая роль уже существует!");
			if (!ModelState.IsValid)
				return View(role);

			var newRole = new IdentityRole { Name = role.Name };
			var result = await rolesManager.CreateAsync(newRole);
			if (result.Succeeded)
				return RedirectToAction(nameof(Index));
			else
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			return View(role);
		}
	}
}
