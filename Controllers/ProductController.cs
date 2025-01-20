using Microsoft.AspNetCore.Mvc;

namespace OnlineShopWebApp.Controllers
{
    public class ProductController : Controller
    {
		IProductsRepository productsRepitory;

        public ProductController(IProductsRepository productsRepository)
        {
            productsRepitory = productsRepository;
        }
        
        public IActionResult Index(int id)
        {
            var product = productsRepitory.TryGetById(id);
            if (product == null)
                return NotFound();
            return View(product);
        }
    }
}
