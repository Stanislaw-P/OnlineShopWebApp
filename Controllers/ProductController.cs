using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;

namespace OnlineShopWebApp.Controllers
{
    public class ProductController : Controller
    {
		IProductsRepository productsRepitory;

        public ProductController(IProductsRepository productsRepository)
        {
            productsRepitory = productsRepository;
        }
        
        public IActionResult Index(Guid id)
        {
            var product = productsRepitory.TryGetById(id);
            if (product == null)
                return NotFound();
            return View(product);
        }
    }
}
