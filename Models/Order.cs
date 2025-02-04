namespace OnlineShopWebApp.Models
{
	public class Order
	{
		public Guid Id { get; set; }
		public UserDeliveryInfo User { get; set; }
		public List<CartItem> Items { get; set; }
		public DateTime Time { get; set; }
		public decimal Cost => Items?.Sum(cartItem => cartItem.Cost) ?? 0;
		public OrderStatus CurrentStatus { get; set; }


		public Order()
		{
			Id = Guid.NewGuid();
			Time = DateTime.Now;
			CurrentStatus = OrderStatus.Created;
		}
	}
}
