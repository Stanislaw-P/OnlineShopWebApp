using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
	public interface ICartsRepository
	{
		Cart TryGetByUserId(string userId);
		void Add(ProductViewModel product, string UserId);
		void DecreaseAmount(ProductViewModel product, string userId);
        void IcreaseAmount(ProductViewModel product, string userId);
        void ClearCartByUserId(string userId);
	}
}