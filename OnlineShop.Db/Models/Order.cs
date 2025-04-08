using OnlineShopWebApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Db.Models
{
	public class Order
	{
        public Guid Id { get; set; }
        public UserDeliveryInfo User { get; set; }
        public List<CartItem> Items { get; set; }
        public OrderStatus CurrentStatus { get; set; }
        public DateTime CreateTime { get; set; }

        public Order()
        {
            CurrentStatus = OrderStatus.Created;
            CreateTime = DateTime.Now;
        }
    }
}
