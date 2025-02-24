namespace OnlineShopWebApp.Models
{
	public class Wishlist
	{
        public Guid ID { get; set; }
        public string UserId { get; set; }
        public List<ProductViewModel> Items { get; set; }
    }
}
