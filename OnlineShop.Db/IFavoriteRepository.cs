
using Onlineshop.Db.Models;
using OnlineShop.Db.Models;

namespace OnlineShop.Db
{
	public interface IFavoriteRepository
	{
		Task Addsync(Product product, string userId);
		Task ClearByIdAsync(string userId);
		Task RemoveAsync(Product product, string userId);
		Task<List<Product>> GetAllAsync(string userId);
	}
}