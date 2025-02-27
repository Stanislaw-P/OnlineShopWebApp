namespace OnlineShopWebApp.Models
{
	public class CartViewModel
	{
		public Guid Id { get; set; }
        public string UserId { get; set; }
		public List<CartItemViewModel> Items { get; set; }
		public decimal Cost => Items?.Sum(item => item.Cost) ?? 0;
		public int Amount => Items?.Sum(item => item.Amount) ?? 0;
    }
}
