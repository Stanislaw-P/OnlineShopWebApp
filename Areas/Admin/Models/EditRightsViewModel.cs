﻿namespace OnlineShopWebApp.Areas.Admin.Models
{
	public class EditRightsViewModel
	{
		public string UserName { get; set; }
		public string Email { get; set; }
		public List<RoleViewModel> UserRoles { get; set; }
		public List<RoleViewModel> AllRoles { get; set; }
	}
}
