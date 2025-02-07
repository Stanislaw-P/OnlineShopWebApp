namespace OnlineShopWebApp.Models
{
	public class User
	{
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

		public User(string email, string password)
		{
			Id = Guid.NewGuid();
			Email = email;
			Password = password;
		}
	}
}
