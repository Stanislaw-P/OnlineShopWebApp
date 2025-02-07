namespace OnlineShopWebApp.Models
{
	public class UserAccount
	{
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }

        public UserAccount(string email, string password, string name, string surname, string phone)
		{
			Id = Guid.NewGuid();
			Email = email;
			Password = password;
			Name = name;
			Surname = surname;
			Phone = phone;
		}
	}
}
