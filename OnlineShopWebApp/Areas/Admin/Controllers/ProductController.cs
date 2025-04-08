using AutoMapper;
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
		readonly ImagesProvider _imagesProvider;
		readonly IMapper _mapper;

		public ProductController(IProductsRepository productsRepository, IWebHostEnvironment webAppEnvironment, IMapper mapper)
		{
			this.productsRepository = productsRepository;
			this.webAppEnvironment = webAppEnvironment;
			_imagesProvider = new ImagesProvider(this.webAppEnvironment);
			_mapper = mapper;
		}

		public IActionResult Index()
		{
			var productsDb = productsRepository.GetAll();
			return View(_mapper.Map<List<ProductViewModel>>(productsDb));
		}

		public IActionResult Remove(Guid productId)
		{
			productsRepository.RemoveById(productId);
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Update(Guid productId)
		{
			var productDb = productsRepository.TryGetById(productId);
			return View(_mapper.Map<EditProductViewModel>(productDb));
		}

		[HttpPost]
		public IActionResult Update(EditProductViewModel editProduct)
		{
			if (!ModelState.IsValid)
				return View(editProduct);

			if (productsRepository.TryGetById(editProduct.Id) == null)
				return NotFound();

			var addedimagePaths = _imagesProvider.SafeFiles(editProduct.UploadedFiles, ImageFolders.Products);
			editProduct.ImagesPaths = addedimagePaths;

			var productDb = _mapper.Map<Product>(editProduct);
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

			var imagesPaths = _imagesProvider.SafeFiles(newProduct.UploadedFiles, ImageFolders.Products);
			var productDb = _mapper.Map<Product>(newProduct, opts =>
			{
				opts.Items["ImagesPaths"] = imagesPaths;
			});
			productsRepository.Add(productDb);
			return RedirectToAction(nameof(Index));
		}
	}
}
