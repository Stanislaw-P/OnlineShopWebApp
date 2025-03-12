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
		readonly IWebHostEnvironment webAppEnvironment;

		public ProductController(IProductsRepository productsRepository, IWebHostEnvironment webAppEnvironment)
		{
			this.productsRepository = productsRepository;
			this.webAppEnvironment = webAppEnvironment;
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

			if (productsRepository.TryGetById(editProduct.Id) == null)
			{
				return NotFound();


			}
			var productDb = new Product
			{
				Id = editProduct.Id,
				Name = editProduct.Name,
				Cost = editProduct.Cost,
				Description = editProduct.Description
			};

			if (editProduct.UploadedFile != null)
			{
				string productImagePath = Path.Combine(webAppEnvironment.WebRootPath + "/images/products/");
				var fileName = Guid.NewGuid() + "." + editProduct.UploadedFile.FileName.Split('.').Last();

				using(FileStream fileStream = new FileStream(productImagePath + fileName, FileMode.Create))
				{
					editProduct.UploadedFile.CopyTo(fileStream);
				}
				productDb.ImagePath = "/images/products/" + fileName;
			}
			productsRepository.Update(productDb);
			return RedirectToAction(nameof(Index));
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

			if (newProduct.UploadedFile != null)
			{
				string productImagePath = Path.Combine(webAppEnvironment.WebRootPath + "/images/products/");
				if (!Directory.Exists(productImagePath))
					Directory.CreateDirectory(productImagePath);

				var fileName = Guid.NewGuid() + "." + newProduct.UploadedFile.FileName.Split('.').Last();

				using (FileStream fileStream = new FileStream(productImagePath + fileName, FileMode.Create))
				{
					newProduct.UploadedFile.CopyTo(fileStream);
				}
				productDb.ImagePath = "/images/products/" + fileName;
			}
			productsRepository.Add(productDb);
			return RedirectToAction(nameof(Index));
		}
	}
}
