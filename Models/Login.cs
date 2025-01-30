using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
	public class Login
	{
        [Required(ErrorMessage ="Обязательное поле!")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Имя должно содержать от 3 до 25 символов!")]
		[EmailAddress]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Обязательное поле!")]
        [StringLength(100, MinimumLength =6, ErrorMessage ="Пароль должен содержать минимум 6 символов!")]
		public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
