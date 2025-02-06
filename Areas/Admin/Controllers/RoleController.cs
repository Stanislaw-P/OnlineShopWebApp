using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Areas.Admin.Models;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        readonly IRolesRepository rolesRepository;

        public RoleController(IRolesRepository rolesRepository)
        {
            this.rolesRepository = rolesRepository;
        }

        public IActionResult Index()
        {
            List<Role> roles = rolesRepository.GetAll();
            return View(roles);
        }

        public IActionResult Remove(string roleName)
        {
            rolesRepository.Remove(roleName);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Role role)
        {
            if (rolesRepository.TryGetByName(role.Name) != null)
                ModelState.AddModelError("", "Такая роль уже существует!");
            if (!ModelState.IsValid)
                return View(role);
            Role newRole = new Role { Name = role.Name };
            rolesRepository.Add(newRole);
            return RedirectToAction(nameof(Index));
        }
    }
}
