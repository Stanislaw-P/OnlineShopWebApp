using Microsoft.AspNetCore.Identity;
using Onlineshop.Db.Models;
using OnlineShop.Db.Migrations.Identity;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Areas.Admin.Models;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Helpers
{
	public static class MappingHelper
	{
		public static List<ProductViewModel> ToProductViewModels(this List<Product> productsDb)
		{
			List<ProductViewModel> productsViewModels = new List<ProductViewModel>();
			foreach (var productDb in productsDb)
			{
				ProductViewModel productViewModel = ToProductViewModel(productDb);

				productsViewModels.Add(productViewModel);
			}
			return productsViewModels;
		}

		public static ProductViewModel ToProductViewModel(this Product productDb)
		{
			return new ProductViewModel
			{
				Id = productDb.Id,
				Name = productDb.Name,
				Cost = productDb.Cost,
				Description = productDb.Description,
				ImagePath = productDb.ImagePath
			};
		}

		public static CartViewModel ToCartViewModel(this Cart cartDb)
		{
			if (cartDb == null)
				return null;
			return new CartViewModel
			{
				Id = cartDb.Id,
				UserId = cartDb.UserId,
				Items = ToCartItemViewModels(cartDb.Items)
			};
		}

		public static List<CartItemViewModel> ToCartItemViewModels(this List<CartItem> cartDtItems)
		{
			var cartItems = new List<CartItemViewModel>();
			foreach (var cartDbItem in cartDtItems)
			{
				var cartItem = new CartItemViewModel
				{
					Id = cartDbItem.Id,
					Amount = cartDbItem.Amount,
					Product = ToProductViewModel(cartDbItem.Product)
				};
				cartItems.Add(cartItem);
			}
			return cartItems;
		}

		public static OrderViewModel ToOrderViewModel(this Order orderDb)
		{
			return new OrderViewModel
			{
				Id = orderDb.Id,
				User = orderDb.User.ToUserDeliveryInfoViewMode(),
				Items = ToCartItemViewModels(orderDb.Items),
				CurrentStatus = (OrderStatusViewModel)(int)orderDb.CurrentStatus,
				Time = orderDb.CreateTime
			};
		}

		public static UserDeliveryInfoViewModel ToUserDeliveryInfoViewMode(this UserDeliveryInfo userDeliveryInfoDb)
		{
			return new UserDeliveryInfoViewModel
			{
				Name = userDeliveryInfoDb.Name,
				Address = userDeliveryInfoDb.Address,
				Phone = userDeliveryInfoDb.Phone
			};
		}

		public static UserDeliveryInfo ToUser(this UserDeliveryInfoViewModel user)
		{
			return new UserDeliveryInfo
			{
				Name = user.Name,
				Address = user.Address,
				Phone = user.Phone
			};
		}

		public static UserViewModel ToUserViewModel(this User user)
		{
			return new UserViewModel
			{
				Email = user.Email,
				Name = user.UserName,
				Phone = user.PhoneNumber
			};
		}

		public static RoleViewModel ToRoleViewModel(this IdentityRole identityRole)
		{
			return new RoleViewModel { Name = identityRole.Name };
		}
	}
}
