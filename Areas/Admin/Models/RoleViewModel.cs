﻿using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Areas.Admin.Models
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Обязательное поле!")]
        public string Name { get; set; }

		public override bool Equals(object? obj)
		{
			var role = (RoleViewModel)obj;
			return role?.Name == Name;
		}
	}
}
