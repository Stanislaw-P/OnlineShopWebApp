using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
	public interface IWishlistsRepository
	{
		Wishlist? TryGetByUserId(string userId);
		void Add(Product product, string userId);
		void RemoveProductByUserId(Product product, string userId);
	}
}