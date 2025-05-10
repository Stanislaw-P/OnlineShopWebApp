using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Onlineshop.Db.Models;
using OnlineShop.Db;
using OnlineShopWebApp.Areas.Admin.Models;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Models;
using System.Data;

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

        public async Task<IActionResult> Index()
        {
            var productsDb = await productsRepository.GetAllAsync();
            return View(_mapper.Map<List<ProductViewModel>>(productsDb));
        }

        public async Task<IActionResult> RemoveAsync(Guid productId)
        {
            await productsRepository.RemoveByIdAsync(productId);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateAsync(Guid productId)
        {
            var productDb = await productsRepository.TryGetByIdAsync(productId);
            return View(_mapper.Map<EditProductViewModel>(productDb));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAsync(EditProductViewModel editProduct)
        {
            if (ModelState.IsValid)
            {
                var productToUpdate = await productsRepository.TryGetByIdAsync(editProduct.Id);
                if (productToUpdate == null)
                {
                    ModelState.AddModelError(string.Empty, "Невозможно сохранить изменения. Данный продукт был удален другим пользователем");
                    return View(editProduct);
                }

                if (editProduct.UploadedFiles != null)
                {
                    var addedimagePaths = await _imagesProvider.SafeFilesAsync(editProduct.UploadedFiles, ImageFolders.Products);
                    editProduct.ImagesPaths = addedimagePaths;
                }
                else
                    editProduct.ImagesPaths = productToUpdate.Images.Select(img => img.URL).ToList();
                var productDb = _mapper.Map<Product>(editProduct);

                try
                {
                    await productsRepository.UpdateAsync(productDb);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exeptionEntry = ex.Entries.Single();
                    var clientValues = (Product)exeptionEntry.Entity;
                    var databaseEntry = await exeptionEntry.GetDatabaseValuesAsync();

                    if (databaseEntry == null)
                        ModelState.AddModelError(string.Empty, "Невозможно сохранить изменения. Данный продукт был удален другим пользователем");
                    else
                    {
                        var databaseValues = (Product)databaseEntry.ToObject();
                        if (databaseValues.Name != editProduct.Name)
                            ModelState.AddModelError("Name", $"Текущее значение: {databaseValues.Name}");
                        if (databaseValues.Description != editProduct.Description)
                            ModelState.AddModelError("Description", $"Текущее значение: {databaseValues.Description}");
                        if (databaseValues.Cost != editProduct.Cost)
                            ModelState.AddModelError("Cost", $"Текущее значение: {databaseValues.Cost}");
                        ModelState.AddModelError(string.Empty, "Запись, которую вы пытаетесь отредактировать была изменена другим пользователем.\nОперация нуждается в подтверждении.");
                        editProduct.ConcurrenceToken = databaseValues.ConcurrenceToken;
                        ModelState.Remove("ConcurrenceToken");
                    }
                }
                return View(editProduct);
            }
            return View(editProduct);
        }


        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(AddProductViewModel newProduct)
        {
            if (!ModelState.IsValid)
                return View(newProduct);

            var imagesPaths = _imagesProvider.SafeFilesAsync(newProduct.UploadedFiles, ImageFolders.Products);
            var productDb = _mapper.Map<Product>(newProduct, opts =>
            {
                opts.Items["ImagesPaths"] = imagesPaths;
            });
            await productsRepository.AddAsync(productDb);
            return RedirectToAction(nameof(Index));
        }
    }
}
