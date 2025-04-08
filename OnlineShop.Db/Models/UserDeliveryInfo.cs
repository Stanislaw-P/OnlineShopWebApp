using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Db.Models
{
	public class UserDeliveryInfo
	{
        public Guid Id { get; set; }
        public string Name { get; set; }
		public string Phone { get; set; }
        public string Address { get; set; }
		public Guid UserAccountId { get; set; }
	}
}
