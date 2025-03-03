using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShopWebApp.Helpers;

namespace OnlineShopWebApp.Controllers
{
    public class ProductController : Controller
    {
		IProductsRepository productsRepitory;

        public ProductController(IProductsRepository productsRepository)
        {
            productsRepitory = productsRepository;
        }
        
        public IActionResult Index(Guid productId)
        {
            var product = productsRepitory.TryGetById(productId);
            if (product == null)
                return NotFound();
            return View(product.ToProductViewModel());
        }
    }
}
