using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
	public class AdminController : Controller
	{
		readonly IProductsRepository productsRepository;

		public AdminController(IProductsRepository productsRepository)
		{
			this.productsRepository = productsRepository;
		}

		public IActionResult Orders()
		{
			return View();
		}

		public IActionResult Users()
		{
			return View();
		}

		public IActionResult Roles()
		{
			return View();
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

			if(productsRepository.TryGetById(editProduct.Id) != null)
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
