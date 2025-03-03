namespace OnlineShopWebApp.Models
{
	public class OrderViewModel
	{
		public Guid Id { get; set; }
		public UserDeliveryInfoViewModel User { get; set; }
		public List<CartItemViewModel> Items { get; set; }
		public DateTime Time { get; set; }
		public decimal Cost => Items?.Sum(cartItem => cartItem.Cost) ?? 0;
		public OrderStatusViewModel CurrentStatus { get; set; }


		public OrderViewModel()
		{
			Id = Guid.NewGuid();
			Time = DateTime.Now;
			CurrentStatus = OrderStatusViewModel.Created;
		}
	}
}
