using Microsoft.AspNetCore.Mvc;

namespace OnlineShopWebApp.Controllers
{
    public class ProductController : Controller
    {
        static ProductsRepository productRepitory;

        public ProductController()
        {
            productRepitory = new ProductsRepository();
        }
        
        public IActionResult Index(int id)
        {
            var product = productRepitory.TryGetById(id);
            if (product == null)
                return NotFound();
            return View(product);
        }
    }
}
