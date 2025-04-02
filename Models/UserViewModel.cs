using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
	public class UserViewModel
	{
		public Guid Id { get; set; }
		public string? Name { get; set; }
		public string? Surname { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
	}
}
