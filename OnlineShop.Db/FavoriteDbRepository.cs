using Microsoft.EntityFrameworkCore;
using Onlineshop.Db.Models;
using OnlineShop.Db.Models;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Db
{
	public class FavoriteDbRepository : IFavoriteRepository
	{
		readonly DatabaseContext databaseContext;

		public FavoriteDbRepository(DatabaseContext databaseContext)
		{
			this.databaseContext = databaseContext;
		}

		public void Add(Product product, string userId)
		{
			var existingFavoriteProduct = databaseContext.FavoriteProducts.FirstOrDefault(favPr => favPr.Product.Id == product.Id && favPr.UserId == userId);
			if (existingFavoriteProduct == null)
			{
				databaseContext.FavoriteProducts.Add(new FavoriteProduct { Product = product, UserId = userId });
				databaseContext.SaveChanges();
			}
		}

		public void Remove(Product product, string userId)
		{
			var existingFavoriteProduct = databaseContext.FavoriteProducts.FirstOrDefault(favPr => favPr.Product.Id == product.Id && favPr.UserId == userId);
			if (existingFavoriteProduct != null)
			{
				databaseContext.FavoriteProducts.Remove(existingFavoriteProduct);
				databaseContext.SaveChanges();
			}
		}

		public void ClearById(string userId)
		{
			var userFavoriteProducts = databaseContext.FavoriteProducts.Where(favPr => favPr.UserId == userId);
			databaseContext.FavoriteProducts.RemoveRange(userFavoriteProducts);
			databaseContext.SaveChanges();
		}

		public List<Product> GetAll(string userId)
		{
			return databaseContext.FavoriteProducts.Where(favPr => favPr.UserId == userId)
				.Include(favPr => favPr.Product)
				.Select(favPr => favPr.Product)
				.ToList();
		}

		//public FavoriteProduct? TryGetByUserId(string userId)
		//{
		//	return databaseContext.FavoriteProducts.Include(x => x.Product).FirstOrDefault(x => x.UserId == userId);
		//}
	}
}
