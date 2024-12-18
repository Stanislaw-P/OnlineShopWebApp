using Microsoft.AspNetCore.Mvc;

namespace OnlineShopWebApp.Controllers
{
    public class ProductController : Controller
    {
        ProductRepitory productRepitory;

        public ProductController()
        {
            productRepitory = new ProductRepitory();
        }

        public string Index(int id)
        {
            var product = productRepitory.TryGetById(id);
            string result = product?.ToString() ?? "Продукта с таким ID нет!";
            return result;
        }
    }
}
