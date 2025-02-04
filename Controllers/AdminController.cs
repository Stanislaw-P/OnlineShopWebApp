using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
	public class AdminController : Controller
	{
		readonly IProductsRepository productsRepository;
		readonly IOrdersRepository ordersRepository;
		readonly IRolesRepository rolesRepository;

		public AdminController(IProductsRepository productsRepository, IOrdersRepository ordersRepository, IRolesRepository rolesRepository)
		{
			this.productsRepository = productsRepository;
			this.ordersRepository = ordersRepository;
			this.rolesRepository = rolesRepository;
		}

		public IActionResult Orders()
		{
			List<Order> orders = ordersRepository.GetAll();
			return View(orders);
		}

		public IActionResult OrderDetails(Guid orderId)
		{
			Order? existingOrder = ordersRepository.TryGetById(orderId);
			if (existingOrder == null)
				return NotFound();
			ViewBag.OrderNumber = ordersRepository.GetAll().IndexOf(existingOrder) + 1;
			return View(existingOrder);
		}

		public IActionResult UpdateOrderStatus(Guid orderId, OrderStatus newStatus)
		{
			ordersRepository.UpdateStatus(orderId, newStatus);
			return RedirectToAction("Orders");
		}

		public IActionResult Users()
		{
			return View();
		}

		public IActionResult Roles()
		{
			List<Role> roles = rolesRepository.GetAll();
			return View(roles);
		}

		public IActionResult RemoveRole(string roleName)
		{
			rolesRepository.Remove(roleName);
			return RedirectToAction("Roles");
		}

		public IActionResult AddRole()
		{
			return View();
		}

		[HttpPost]
		public IActionResult AddRole(Role role)
		{
			if (rolesRepository.TryGetByName(role.Name) != null)
				ModelState.AddModelError("", "Такая роль уже существует!");
			if (!ModelState.IsValid)
				return View(role);
			Role newRole = new Role { Name = role.Name };
			rolesRepository.Add(newRole);
			return RedirectToAction("Roles");
		}

		public IActionResult Products()
		{
			List<Product> products = productsRepository.GetAll();
			return View(products);
		}

		public IActionResult DeleteProduct(int productId)
		{
			List<Product> products = productsRepository.GetAll();
			products.RemoveAll(product => product.Id == productId);
			return RedirectToAction("Products");
		}

		public IActionResult EditProduct(int productId)
		{
			Product product = productsRepository.TryGetById(productId);
			return View(product);
		}

		[HttpPost]
		public IActionResult EditProduct(Product editProduct)
		{
			if (!ModelState.IsValid)
				return View(editProduct);

			if (productsRepository.TryGetById(editProduct.Id) != null)
			{
				productsRepository.EditById(editProduct);
				return RedirectToAction("Products");
			}
			return NotFound();
		}

		public IActionResult AddProduct()
		{
			return View();
		}

		[HttpPost]
		public IActionResult AddProduct(Product newProduct)
		{
			if (!ModelState.IsValid)
				return View(newProduct);

			productsRepository.Add(newProduct);
			return RedirectToAction("Products");
		}
	}
}
