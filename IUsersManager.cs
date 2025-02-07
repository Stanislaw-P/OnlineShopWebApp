using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
	public interface IUsersManager
	{
		void Add(UserAccount newUser);
		UserAccount? TryGetByEmail(string email);
	}
}