using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using OnlineShop.Db.Models;

namespace OnlineShop.Db
{
	public class IdentityInitializer
	{
		public static void Initialize(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
		{
			var adminEmail = "admin@gmail.com";
			var password = "_Aa123456";
			if (roleManager.FindByNameAsync(Constants.AdminRoleName).Result == null)
				roleManager.CreateAsync(new IdentityRole(Constants.AdminRoleName)).Wait();
			if (roleManager.FindByIdAsync(Constants.UserRoleName).Result == null)
				roleManager.CreateAsync(new IdentityRole(Constants.UserRoleName)).Wait();
			if(userManager.FindByEmailAsync(adminEmail).Result == null)
			{
				var admin = new User { Email = adminEmail, UserName = adminEmail };
				var result = userManager.CreateAsync(admin, password).Result;
				if (result.Succeeded)
				{
					// Добавляем роль, которую ранее записали в базу данныз, иначе будет ошибка
					userManager.AddToRoleAsync(admin, Constants.AdminRoleName).Wait();
				}
			}
		}
	}
}
