using OnlineShop.Db.Models;

namespace OnlineShopWebApp
{
	public interface IOrdersRepository
	{
		Task AddAsync(Order order);
		Task<List<Order>> GetAllAsync();
		Task<List<Order>?> TryGetUserOrdersAsync(Guid userAccountId);
		Task<Order?> TryGetByIdAsync(Guid id);
		Task UpdateStatusAsync(Guid orderId, OrderStatus status);
	}
}