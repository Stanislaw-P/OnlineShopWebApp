using Onlineshop.Db.Models;
using OnlineShop.Db.Models;

namespace OnlineShop.Db
{
	public interface ICartsRepository
	{
		Task<Cart?> TryGetByUserIdAsync(string userId);
		Task AddAsync(Product product, string UserId);
		Task DecreaseAmountAsync(Product product, string userId);
        Task IcreaseAmounAsync(Product product, string userId);
        Task ClearCartByUserIdAsync(string userId);
	}
}