using Microsoft.AspNetCore.Mvc;
using Onlineshop.Db.Models;
using OnlineShop.Db;
using OnlineShopWebApp.Areas.Admin.Models;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
	[Area("Admin")]
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
			List<ProductViewModel> productViewModels = new List<ProductViewModel>();
			foreach (var product in productsDb)
			{
				ProductViewModel productViewModel = new ProductViewModel
				{
					Name = product.Name,
					Cost = product.Cost,
					Description = product.Description,
					ImagePath = product.ImagePath
				};
				productViewModels.Add(productViewModel);
			}
			return View(productViewModels);
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
				Name = newProduct.Name,
				Cost = newProduct.Cost,
				Description = newProduct.Description
			};
			productsRepository.Add(productDb);
			return RedirectToAction(nameof(Index));
		}
	}
}
