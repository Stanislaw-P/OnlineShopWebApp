using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
	public class Role
	{
		[Required(ErrorMessage = "Обязательное поле!")]
		public string Name { get; set; }
	}
}
