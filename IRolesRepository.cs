using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
	public interface IRolesRepository
	{
		List<Role> GetAll();
		Role TryGetByName(string name);
		void Add(Role role);
		void Remove(string name);
	}
}