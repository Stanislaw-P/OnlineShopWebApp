
using Microsoft.EntityFrameworkCore;
using Onlineshop.Db.Models;

namespace OnlineShop.Db
{
	public class ProductsDbRepository : IProductsRepository
	{
		readonly DatabaseContext databaseContext;

		public ProductsDbRepository(DatabaseContext databaseContext)
		{
			this.databaseContext = databaseContext;
		}

		public List<Product> GetAll()
		{
			return databaseContext.Products.Include(p => p.Images).ToList();
		}

		public Product? TryGetById(Guid id)
		{
			return databaseContext.Products
				.Include(x => x.Images)
				.FirstOrDefault(product => product.Id == id);
		}

		public void Update(Product editProduct)
		{
			Product? existingProduct = databaseContext.Products
				.Include(x => x.Images)
				.FirstOrDefault(product => product.Id == editProduct.Id);

			if (existingProduct == null)
				return;

			existingProduct.Name = editProduct.Name;
			existingProduct.Cost = editProduct.Cost;
			existingProduct.Description = editProduct.Description;

			foreach (var image in editProduct.Images)
			{
				image.ProductId = editProduct.Id;
				databaseContext.Images.Add(image);
			}

			databaseContext.SaveChanges();
		}

		public void Add(Product product)
		{
			databaseContext.Products.Add(product);
			databaseContext.SaveChanges();
		}

		public void RemoveById(Guid id)
		{
			var productForRemove = databaseContext.Products.FirstOrDefault(product => product.Id == id);
			if (productForRemove == null)
				return;
			databaseContext.Products.Remove(productForRemove);
			databaseContext.SaveChanges();
		}
	}
}
