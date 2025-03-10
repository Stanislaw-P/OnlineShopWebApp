using OnlineShopWebApp.Areas.Admin.Models;

namespace OnlineShopWebApp
{
    public class InMemoryRolesRepository : IRolesRepository
	{
		private List<RoleViewModel> roles = new List<RoleViewModel>();

		public void Add(RoleViewModel role)
		{
			roles.Add(role);
		}

		public List<RoleViewModel> GetAll()
		{
			return roles;
		}

		public RoleViewModel? TryGetByName(string name)
		{
			return roles.FirstOrDefault(role => role.Name == name);
		}

		public void Remove(string name)
		{
			roles.RemoveAll(role => role.Name == name);
		}
	}
}
