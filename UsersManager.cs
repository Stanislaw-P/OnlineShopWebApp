using OnlineShopWebApp.Areas.Admin.Models;
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

		public UserAccount? TryGetById(Guid id)
		{
			return users.FirstOrDefault(user => user.Id == id);
		}

		public void ChangePassword(NewUserPassword newUserPassword)
		{
			UserAccount? existingUser = TryGetByEmail(newUserPassword.Email);
			if (existingUser == null)
				return;
			existingUser.Password = newUserPassword.Password;
		}

		public void EditUser(UserAccount editUser)
		{
			UserAccount? existingUser = TryGetById(editUser.Id);
			if (existingUser == null)
				return;
			existingUser.Name = editUser.Name;
			existingUser.Surname = editUser.Surname;
			existingUser.Email = editUser.Email;
			existingUser.Phone = editUser.Phone;
		}

		public void RemoveById(Guid id)
		{
			users.RemoveAll(user => user.Id == id);
		}
	}
}
