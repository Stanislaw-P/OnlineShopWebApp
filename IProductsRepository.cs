using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
	public interface IProductsRepository
	{
		List<Product> GetAll();
		Product TryGetById(int id);
		void EditById(Product product);
		void Add(Product product);
	}
}