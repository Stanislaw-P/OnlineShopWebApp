using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
	public class InMemoryOrdersRepository : IOrdersRepository
	{
		List<Order> orders = new List<Order>();

		public void Add(Order order)
		{
			orders.Add(order);
		}
	}
}
