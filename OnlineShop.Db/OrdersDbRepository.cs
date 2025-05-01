using Microsoft.EntityFrameworkCore;
using OnlineShop.Db;
using OnlineShop.Db.Models;

namespace OnlineShopWebApp
{
	public class OrdersDbRepository : IOrdersRepository
	{
		readonly DatabaseContext databaseContext;

		public OrdersDbRepository(DatabaseContext databaseContext)
		{
			this.databaseContext = databaseContext;
		}

		public async Task AddAsync(Order order)
		{
			await databaseContext.Orders.AddAsync(order);
			await databaseContext.SaveChangesAsync();
		}

		public async Task<List<Order>> GetAllAsync()
		{
			return await databaseContext.Orders
				.Include(x => x.User)
				.Include(x => x.Items)
				.ThenInclude(x => x.Product)
				.ToListAsync();
		}

		public async Task<Order?> TryGetByIdAsync(Guid id)
		{
			return await databaseContext.Orders
				.Include(x => x.User)
				.Include(x => x.Items)
				.ThenInclude(x => x.Product)
				.FirstOrDefaultAsync(order => order.Id == id);
		}

		public async Task<List<Order>?> TryGetUserOrdersAsync(Guid userAccountId)
		{
			return await databaseContext.Orders
				.Include(x => x.User)
				.Include(x => x.Items)
				.ThenInclude(x => x.Product)
				.Where(order => order.User.UserAccountId == userAccountId)
				.ToListAsync();
		}

		public async Task UpdateStatusAsync(Guid orderId, OrderStatus newStatus)
		{
			var order = await TryGetByIdAsync(orderId);
			if (order != null)
			{
				order.CurrentStatus = newStatus;
				await databaseContext.SaveChangesAsync();
			}
		}
	}
}
