using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
	public interface IUsersManager
	{
		List<UserAccount> GetAll();
		void Add(UserAccount newUser);
		UserAccount? TryGetByEmail(string email);
	}
}