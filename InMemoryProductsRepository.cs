using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
    public class InMemoryProductsRepository : IProductsRepository
	{
        List<Product> products = new List<Product>() {
            new Product("Ice-Latte", 250, "Coffe", "/images/armchair blue.jpg"),
            new Product("Milka", 100, "Chocolate", "/images/new year ball.jpeg"),
            new Product("Alpen gold", 65, "Chocolate", "/images/rabbit.jpg"),
            new Product("Milk", 70, "Osetia", "/images/milk.png"),
            new Product("Apple", 100, "Green Apple", "/images/apple.jpg")
        };

        public List<Product> GetAll()
        {
            return products;
        }

		public Product? TryGetById(int id)
        {
            return products.FirstOrDefault(product => product.Id == id);
        }

        public void EditById(Product editProduct)
        {
            Product existingProduct = products.FirstOrDefault(product => product.Id == editProduct.Id);
            if (existingProduct == null)
                return;
			existingProduct.Name = editProduct.Name;
			existingProduct.Cost = editProduct.Cost;
			existingProduct.Description = editProduct.Description;
			existingProduct.ImagePath = editProduct.ImagePath;
        }

        public void Add(Product product)
        {
			products.Add(product);
        }
    }
}
