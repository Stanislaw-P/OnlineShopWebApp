using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
	public interface IProductsRepository
	{
		List<Product> GetAll();
		Product TryGetById(int id);
		void EditById(EditProduct product);
	}
}