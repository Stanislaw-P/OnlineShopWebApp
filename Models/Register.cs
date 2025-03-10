using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
    public class Register
    {
		[Required(ErrorMessage = "Обязательное поле!")]
		[StringLength(25, MinimumLength = 3, ErrorMessage = "Поле должно содержать от 3 до 25 символов!")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Обязательное поле!")]
		[StringLength(25, MinimumLength = 3, ErrorMessage = "Поле должно содержать от 3 до 25 символов!")]
		public string Surname { get; set; }

		[Required(ErrorMessage = "Обязательное поле!")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Почта должна содержать от 3 до 30 символов!")]
        [EmailAddress]
        public string Email { get; set; }

		[Required(ErrorMessage = "Обязательное поле!")]
        [RegularExpression(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$", ErrorMessage = "Неверный формат номера телефона.")]
		public string Phone { get; set; }

		[Required(ErrorMessage = "Обязательное поле!")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен содержать минимум 6 символов!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
        [Compare("Password", ErrorMessage = "Пароли должны сопвадать!")]
        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }
    }
}
