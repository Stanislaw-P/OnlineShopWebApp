using OnlineShopWebApp.Areas.Admin.Models;

namespace OnlineShopWebApp
{
    public interface IRolesRepository
	{
		List<RoleViewModel> GetAll();
		RoleViewModel TryGetByName(string name);
		void Add(RoleViewModel role);
		void Remove(string name);
	}
}