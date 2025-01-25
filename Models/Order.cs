namespace OnlineShopWebApp.Models
{
	public class Order
	{
		public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public Cart Cart { get; set; }
    }
}
