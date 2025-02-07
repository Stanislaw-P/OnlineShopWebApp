using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
	public class InMemoryUsersRepository : IUsersRepository
	{
		readonly List<User> users = new List<User>();

		public void Add(User newUser)
		{
			users.Add(newUser);
		}

		public User? TryGetByEmail(string email)
		{
			return users.FirstOrDefault(user => user.Email == email);
		}

		public bool PasswordIsCorrect(User? user, string password)
		{
			return user?.Password == password;
		}
	}
}
