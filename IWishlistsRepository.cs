using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
	public interface IWishlistsRepository
	{
		Wishlist? TryGetByUserId(string userId);
		void Add(ProductViewModel product, string userId);
		void RemoveProductByUserId(ProductViewModel product, string userId);
	}
}