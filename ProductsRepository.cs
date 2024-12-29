using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
    public class ProductsRepository
    {
        static List<Product> products = new List<Product>() {
            new Product("Coffe", 250, "Ice-Latte", "/images/armchair blue.jpg"),
            new Product("Chocolate", 100, "Milka", "/images/new year ball.jpeg"),
            new Product("Chocolate", 65, "Alpen", "/images/rabbit.jpg"),
            new Product("Milk", 70, "Osetia", "/images/milk.png"),
            new Product("Apple", 100, "Green Apple", "/images/apple.jpg")
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
