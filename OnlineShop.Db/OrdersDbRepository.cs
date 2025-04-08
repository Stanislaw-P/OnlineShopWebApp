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

		public void Add(Order order)
		{
			databaseContext.Orders.Add(order);
			databaseContext.SaveChanges();
		}

		public List<Order> GetAll()
		{
			return databaseContext.Orders.Include(x => x.User)
				.Include(x => x.Items)
				.ThenInclude(x => x.Product)
				.ToList();
		}

		public Order? TryGetById(Guid id)
		{
			return databaseContext.Orders.Include(x => x.User).Include(x => x.Items).ThenInclude(x => x.Product).FirstOrDefault(order => order.Id == id);
		}

		public List<Order>? TryGetUserOrders(Guid userAccountId)
		{
			return databaseContext.Orders.Include(x => x.User)
				.Include(x => x.Items)
				.ThenInclude(x => x.Product)
				.Where(order => order.User.UserAccountId == userAccountId)
				.ToList();
		}

		public void UpdateStatus(Guid orderId, OrderStatus newStatus)
		{
			var order = TryGetById(orderId);
			if (order != null)
			{
				order.CurrentStatus = newStatus;
				databaseContext.SaveChanges();
			}
		}
	}
}
