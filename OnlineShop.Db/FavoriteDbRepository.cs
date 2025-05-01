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

		public async Task Addsync(Product product, string userId)
		{
			var existingFavoriteProduct = await databaseContext.FavoriteProducts
				.FirstOrDefaultAsync(favPr => favPr.Product.Id == product.Id && favPr.UserId == userId);
			if (existingFavoriteProduct == null)
			{
				await databaseContext.FavoriteProducts.AddAsync(new FavoriteProduct { Product = product, UserId = userId });
				await databaseContext.SaveChangesAsync();
			}
		}

		public async Task RemoveAsync(Product product, string userId)
		{
			var existingFavoriteProduct = await databaseContext.FavoriteProducts
				.FirstOrDefaultAsync(favPr => favPr.Product.Id == product.Id && favPr.UserId == userId);
			if (existingFavoriteProduct != null)
			{
				databaseContext.FavoriteProducts.Remove(existingFavoriteProduct);
				await databaseContext.SaveChangesAsync();
			}
		}

		public async Task ClearByIdAsync(string userId)
		{
			var userFavoriteProducts = databaseContext.FavoriteProducts.Where(favPr => favPr.UserId == userId);
			databaseContext.FavoriteProducts.RemoveRange(userFavoriteProducts);
			await databaseContext.SaveChangesAsync();
		}

		public async Task<List<Product>> GetAllAsync(string userId)
		{
			return await databaseContext.FavoriteProducts
				.Where(favPr => favPr.UserId == userId)
				.Include(favPr => favPr.Product)
				.Select(favPr => favPr.Product)
				.ToListAsync();
		}
	}
}
