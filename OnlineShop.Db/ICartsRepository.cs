using Onlineshop.Db.Models;
using OnlineShop.Db.Models;

namespace OnlineShop.Db
{
	public interface ICartsRepository
	{
		Cart TryGetByUserId(string userId);
		void Add(Product product, string UserId);
		void DecreaseAmount(Product product, string userId);
        void IcreaseAmount(Product product, string userId);
        void ClearCartByUserId(string userId);
	}
}