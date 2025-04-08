using Onlineshop.Db.Models;

namespace OnlineShop.Db
{
	public interface IProductsRepository
	{
		List<Product> GetAll();
		Product TryGetById(Guid id);
		void Update(Product product);
		void Add(Product product);
		void RemoveById(Guid id);
	}
}