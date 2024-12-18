using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
    public class ProductRepitory
    {
        static List<Product> products = new List<Product>() {
            new Product("Coffe", 250, "Ice-Latte"),
            new Product("Chocolate", 100, "Milka"),
            new Product("Chocolate", 65, "Alpen")
        };

        public List<Product> GetAll()
        {
            return products;
        }

        public Product TryGetById(int id)
        {
            return products.FirstOrDefault(product => product.Id == id);
        }
    }
}
