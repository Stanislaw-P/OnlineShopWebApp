
using Microsoft.EntityFrameworkCore;
using Onlineshop.Db.Models;
using System.Threading.Tasks;

namespace OnlineShop.Db
{
    public class ProductsDbRepository : IProductsRepository
    {
        readonly DatabaseContext databaseContext;

        public ProductsDbRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await databaseContext.Products.Include(p => p.Images).ToListAsync();
        }

        public async Task<Product?> TryGetByIdAsync(Guid id)
        {
            return await databaseContext.Products
                .Include(x => x.Images)
                .FirstOrDefaultAsync(product => product.Id == id);
        }

        public async Task UpdateAsync(Product editProduct)
        {
            Product? existingProduct = await TryGetByIdAsync(editProduct.Id);

            if (existingProduct == null)
                return;
            databaseContext.Entry(existingProduct).Property("ConcurrenceToken").OriginalValue = editProduct.ConcurrenceToken;
            existingProduct.Name = editProduct.Name;
            existingProduct.Cost = editProduct.Cost;
            existingProduct.Description = editProduct.Description;

            foreach (var image in editProduct.Images)
            {
                if (existingProduct.Images.Count(img => img.URL == image.URL) == 0)
                {
                    image.ProductId = editProduct.Id;
                    await databaseContext.Images.AddAsync(image);
                }
            }

            await databaseContext.SaveChangesAsync();
        }

        public async Task AddAsync(Product product)
        {
            await databaseContext.Products.AddAsync(product);
            await databaseContext.SaveChangesAsync();
        }

        public async Task RemoveByIdAsync(Guid id)
        {
            var productForRemove = await databaseContext.Products.FirstOrDefaultAsync(product => product.Id == id);
            if (productForRemove == null)
                return;
            databaseContext.Products.Remove(productForRemove);
            await databaseContext.SaveChangesAsync();
        }
    }
}
