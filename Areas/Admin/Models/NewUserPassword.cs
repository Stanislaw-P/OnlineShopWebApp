using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Areas.Admin.Models
{
	public class NewUserPassword
	{
		public Guid Id { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
		[StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен содержать минимум 6 символов!")]
		public string Password { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают!")]
        public string ConfirmPassword { get; set; }
    }
}
