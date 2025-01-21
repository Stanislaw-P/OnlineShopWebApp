using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
	public interface ICartsRepository
	{
		Cart TryGetByUserId(string userId);
		void Add(Product product, string UserId);
		void DecreaseAmount(Product product, string userId);
		void Clear(string userId);
	}
}