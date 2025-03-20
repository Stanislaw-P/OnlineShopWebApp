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
		readonly ImagesProvider imagesProvider;

		public ProductController(IProductsRepository productsRepository, IWebHostEnvironment webAppEnvironment)
		{
			this.productsRepository = productsRepository;
			this.webAppEnvironment = webAppEnvironment;
			imagesProvider = new ImagesProvider(this.webAppEnvironment);
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
			return View(productDb.ToEditProductViewModel());
		}

		[HttpPost]
		public IActionResult Update(EditProductViewModel editProduct)
		{
			if (!ModelState.IsValid)
				return View(editProduct);

			if (productsRepository.TryGetById(editProduct.Id) == null)
				return NotFound();

			var addedimagePaths = imagesProvider.SafeFiles(editProduct.UploadedFiles, ImageFolders.Products);
			editProduct.ImagesPaths = addedimagePaths;

			var productDb = editProduct.ToProduct();
			productsRepository.Update(productDb);
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Add(AddProductViewModel newProduct)
		{
			if (!ModelState.IsValid)
				return View(newProduct);

			var imagesPaths = imagesProvider.SafeFiles(newProduct.UploadedFiles, ImageFolders.Products);
			productsRepository.Add(newProduct.ToProduct(imagesPaths));
			return RedirectToAction(nameof(Index));
		}
	}
}
