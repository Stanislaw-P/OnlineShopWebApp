using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Onlineshop.Db.Models;
using OnlineShop.Db;
using OnlineShopWebApp.Areas.Admin.Models;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
	[Area(Constants.AdminRoleName)]
	[Authorize(Roles = Constants.AdminRoleName)]
	public class ProductController : Controller
	{
		readonly IProductsRepository productsRepository;

		public ProductController(IProductsRepository productsRepository)
		{
			this.productsRepository = productsRepository;
		}

		public IActionResult Index()
		{
			var productsDb = productsRepository.GetAll();
			return View(productsDb.ToProductViewModels());
		}

		public IActionResult Remove(Guid productId)
		{
			productsRepository.RemoveById(productId);
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Update(Guid productId)
		{
			var productDb = productsRepository.TryGetById(productId);
			ProductViewModel productViewModel = new ProductViewModel
			{
				Id = productDb.Id,
				Name = productDb.Name,
				Description = productDb.Description,
				Cost = productDb.Cost,
				ImagePath = productDb.ImagePath
			};
			return View(productViewModel);
		}

		[HttpPost]
		public IActionResult Update(ProductViewModel editProduct)
		{
			if (!ModelState.IsValid)
				return View(editProduct);

			if (productsRepository.TryGetById(editProduct.Id) != null)
			{
				var productDb = new Product
				{
					Id = editProduct.Id,
					Name = editProduct.Name,
					Cost = editProduct.Cost,
					Description = editProduct.Description
				};
				productsRepository.Update(productDb);
				return RedirectToAction(nameof(Index));
			}
			return NotFound();
		}

		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Add(ProductViewModel newProduct)
		{
			if (!ModelState.IsValid)
				return View(newProduct);

			var productDb = new Product
			{
				Id = newProduct.Id,
				Name = newProduct.Name,
				Cost = newProduct.Cost,
				Description = newProduct.Description
			};
			productsRepository.Add(productDb);
			return RedirectToAction(nameof(Index));
		}
	}
}
