
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using OnlineShop.Db;

namespace OnlineShopWebApp.Helpers
{
	public class ProductCache : BackgroundService
	{
		readonly IServiceProvider serviceProvider;
		readonly IMemoryCache cache;

		public ProductCache(IServiceProvider serviceProvider, IMemoryCache cache)
		{
			this.serviceProvider = serviceProvider;
			this.cache = cache;
		}

		// Вызывается при старте приложения
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			// Пока приложение не закрылось
			while (!stoppingToken.IsCancellationRequested)
			{
				// Кешируем продукты
				CachingAllProducts();

				// Создается задержка на 1 минуту, чтоб каждую мин объекты обновлялись.
				// Старые объекты заменяются на том же ключе
				await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
			}
		}

		void CachingAllProducts()
		{
			using (var scope = serviceProvider.CreateScope())
			{
				var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
				var products = databaseContext.Products.Include(p => p.Images).ToList();

				// Кеширование всех продуктов
				if (products != null)
					cache.Set(Constants.KeyCacheAllProducts, products);

				// Добавление (обновление) каждого продукта в кеш по отдельности, по ключю product.Id
				foreach (var product in products)
				{
					if(product != null)
						cache.Set(product.Id, product);
				}
			}
		}
	}
}
