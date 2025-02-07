namespace OnlineShopWebApp.Models
{
	public class UserAccount
	{
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

		public UserAccount(string email, string password)
		{
			Id = Guid.NewGuid();
			Email = email;
			Password = password;
		}
	}
}
