using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
	public class UsersManager : IUsersManager
	{
		readonly List<UserAccount> users = new List<UserAccount>();

		public List<UserAccount> GetAll()
		{
			return users;
		}

		public void Add(UserAccount newUser)
		{
			users.Add(newUser);
		}

		public UserAccount? TryGetByEmail(string email)
		{
			return users.FirstOrDefault(user => user.Email == email);
		}
	}
}
