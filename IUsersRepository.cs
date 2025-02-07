using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
	public interface IUsersRepository
	{
		void Add(User newUser);
		User? TryGetByEmail(string email);
		bool PasswordIsCorrect(User? user, string password);
	}
}