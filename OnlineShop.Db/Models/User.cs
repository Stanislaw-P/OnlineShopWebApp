using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Db.Models
{
	public class User : IdentityUser
	{
		public string UserSurname { get; set; }
		public AvatarImage Avatar { get; set; }
	}
}
