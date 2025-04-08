using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Areas.Admin.Models
{
	public class EditUserViewModel
	{
		public Guid Id { get; set; }

		[Required(ErrorMessage ="Обязательное поле!")]
		[StringLength(25, MinimumLength = 3, ErrorMessage = "Поле должно содержать от 3 до 25 символов!")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Обязательное поле!")]
		[StringLength(25, MinimumLength = 3, ErrorMessage = "Поле должно содержать от 3 до 25 символов!")]
		public string UserSurname { get; set; }
		
		[Required(ErrorMessage ="Обязательное поле!")]
        [RegularExpression(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$", ErrorMessage = "Неверный формат номера телефона.")]
		public string PhoneNumber { get; set; }
	}
}
