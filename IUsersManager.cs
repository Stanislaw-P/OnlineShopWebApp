using OnlineShopWebApp.Areas.Admin.Models;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
	public interface IUsersManager
	{
		List<UserAccount> GetAll();
		void Add(UserAccount newUser);
		UserAccount? TryGetByEmail(string email);
		UserAccount? TryGetById(Guid id);
		void ChangePassword(NewUserPassword newUserPassword);
		void EditUser(UserAccount editUser);
		void RemoveById(Guid id);
	}
}