
using Onlineshop.Db.Models;
using OnlineShop.Db.Models;

namespace OnlineShop.Db
{
	public interface IFavoriteRepository
	{
		void Add(Product product, string userId);
		public void ClearById(string userId);
		void Remove(Product product, string userId);
		public List<Product> GetAll(string userId);
	}
}