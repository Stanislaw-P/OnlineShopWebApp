using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
	public class InMemoryOrdersRepository : IOrdersRepository
	{
		List<Cart> orders = new List<Cart>();
		public void Add(Cart cart)
		{
			orders.Add(cart);
		}
	}
}
