using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
	public class InMemoryRolesRepository : IRolesRepository
	{
		private List<Role> roles = new List<Role>();

		public void Add(Role role)
		{
			roles.Add(role);
		}

		public List<Role> GetAll()
		{
			return roles;
		}

		public Role? TryGetByName(string name)
		{
			return roles.FirstOrDefault(role => role.Name == name);
		}

		public void Remove(string name)
		{
			roles.RemoveAll(role => role.Name == name);
		}
	}
}
