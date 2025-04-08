using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Db.Models
{
	public class AvatarImage
	{
		public Guid Id { get; set; }
		public string URL { get; set; }
		public string UserId { get; set; }
		public User User { get; set; }
	}
}
