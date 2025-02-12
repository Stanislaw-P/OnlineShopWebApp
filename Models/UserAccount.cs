using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
	public class UserAccount
	{
        public Guid Id { get; set; }
		[Required(ErrorMessage ="Обязательное поле!")]
		[EmailAddress(ErrorMessage ="Введите корректный адрес почты")]
        public string? Email { get; set; }
        public string? Password { get; set; }

		[Required(ErrorMessage = "Обязательное поле!")]
		public string? Name { get; set; }

		[Required(ErrorMessage = "Обязательное поле!")]
		public string? Surname { get; set; }

		[Required(ErrorMessage = "Обязательное поле!")]
		public string? Phone { get; set; }

		public UserAccount()
		{
			Id = Guid.NewGuid();
		}

        public UserAccount(string email, string password, string name, string surname, string phone) :this()
		{
			Email = email;
			Password = password;
			Name = name;
			Surname = surname;
			Phone = phone;
		}
	}
}
