using OnlineShop.Db.Models;
using System.ComponentModel.DataAnnotations;

namespace Onlineshop.Db.Models
{
    public class Product
    {
        public Guid Id { get; set; }
		public string Name { get; set; }
		public decimal Cost { get; set; }
        public string Description { get; set; }
        public List<CartItem> CartItems { get; set; }
		public List<Image> Images { get; set; }

		public Product()
        {
            CartItems = new List<CartItem>();
        }

        public Product(Guid product1Id, string name, decimal cost, string description) : this()
        {
            Id = product1Id;
            Name = name;
            Cost = cost;
            Description = description;
        }
    }
}
