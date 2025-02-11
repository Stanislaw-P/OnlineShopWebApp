using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
	public interface IProductsRepository
	{
		List<Product> GetAll();
		Product TryGetById(int id);
		void Update(Product product);
		void Add(Product product);
		void RemoveById(int id);
	}
}