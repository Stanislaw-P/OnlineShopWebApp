using Onlineshop.Db.Models;

namespace OnlineShop.Db
{
	public interface IProductsRepository
	{
		Task<List<Product>> GetAllAsync();
		Task<Product?> TryGetByIdAsync(Guid id);
		Task UpdateAsync(Product product);
		Task AddAsync(Product product);
		Task RemoveByIdAsync(Guid id);
	}
}