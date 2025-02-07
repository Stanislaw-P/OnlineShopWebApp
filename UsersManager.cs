using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
	public class UsersManager : IUsersManager
	{
		readonly List<UserAccount> users = new List<UserAccount>()
		{
			new UserAccount("test@mail.ru", "123123", "Admin", "Persaev", "+77777777777")
		};

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
