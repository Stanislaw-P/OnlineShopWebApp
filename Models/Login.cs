using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
    public class Login
    {

        [Required(ErrorMessage = "Обязательное поле!")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Почта должна содержать от 3 до 30 символов!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен содержать минимум 6 символов!")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
	}
}
