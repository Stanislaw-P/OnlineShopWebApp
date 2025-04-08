using OnlineShop.Db.Models;

namespace OnlineShopWebApp
{
	public interface IOrdersRepository
	{
		void Add(Order order);
		List<Order> GetAll();
		List<Order>? TryGetUserOrders(Guid userAccountId);
		Order? TryGetById(Guid id);
		void UpdateStatus(Guid orderId, OrderStatus status);
	}
}