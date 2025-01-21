namespace OnlineShopWebApp.Models
{
	public class Cart
	{
		public Guid Id { get; set; }
        public string UserId { get; set; }
		public List<CartItem> Items { get; set; }
		public decimal Cost => Items.Sum(item => item.Cost);

		public void Decreace(int productId)
		{
			CartItem cartItem = Items.FirstOrDefault(cartItem => cartItem.Product.Id == productId);
			cartItem.Amount--;
			if (cartItem.Amount == 0)
				Items.Remove(cartItem);
		}

		public void Clear()
		{
			Items.Clear();
		}
    }
}
