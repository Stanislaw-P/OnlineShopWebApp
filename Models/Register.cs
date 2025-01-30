using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
	public class Register
	{
		[Required(ErrorMessage = "Обязательное поле!")]
		[StringLength(25, MinimumLength = 3, ErrorMessage = "Имя должно содержать от 3 до 25 символов!")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Обязательное поле!")]
		[StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен содержать минимум 6 символов!")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Обязательное поле!")]
		[Compare("Password", ErrorMessage ="Пароли должны сопвадать!")]
        public string ConfirmPassword { get; set; }
    }
}
