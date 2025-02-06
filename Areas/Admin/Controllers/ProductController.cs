using Microsoft.AspNetCore.Mvc;
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
            List<Product> products = productsRepository.GetAll();
            return View(products);
        }

        public IActionResult Delete(int productId)
        {
            List<Product> products = productsRepository.GetAll();
            products.RemoveAll(product => product.Id == productId);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int productId)
        {
            Product product = productsRepository.TryGetById(productId);
            return View(product);
        }

        [HttpPost]
        public IActionResult Update(Product editProduct)
        {
            if (!ModelState.IsValid)
                return View(editProduct);

            if (productsRepository.TryGetById(editProduct.Id) != null)
            {
                productsRepository.EditById(editProduct);
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Product newProduct)
        {
            if (!ModelState.IsValid)
                return View(newProduct);

            productsRepository.Add(newProduct);
            return RedirectToAction(nameof(Index));
        }
    }
}
