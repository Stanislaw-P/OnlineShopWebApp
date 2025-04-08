using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
	public class UserDeliveryInfoViewModel
	{
		[Required(ErrorMessage ="Обязательное поле!")]
		[StringLength(55, MinimumLength =5, ErrorMessage ="ФИО должно содержать минимум 5 символов!")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Обязательное поле!")]
		public string Phone { get; set; }

		[Required(ErrorMessage = "Обязательное поле!")]
		public string Address { get; set; }
	}
}
