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

        public IActionResult Remove(string roleName)
        {
            var existingRole =  rolesManager.FindByNameAsync(roleName).Result;
            if (existingRole != null)
                rolesManager.DeleteAsync(existingRole).Wait();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(RoleViewModel role)
        {
            if (rolesManager.FindByNameAsync(role.Name).Result != null)
                ModelState.AddModelError("", "Такая роль уже существует!");
            if (!ModelState.IsValid)
                return View(role);

            var newRole = new IdentityRole { Name = role.Name };
            var result = rolesManager.CreateAsync(newRole).Result;
            if(result.Succeeded)
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
