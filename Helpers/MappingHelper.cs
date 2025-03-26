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
				ImagePaths = productDb.Images?.Select(image => image.URL)?.ToArray()
			};
		}

		public static EditProductViewModel ToEditProductViewModel(this Product productDb)
		{
			return new EditProductViewModel
			{
				Id = productDb.Id,
				Name = productDb.Name,
				Cost = productDb.Cost,
				Description = productDb.Description,
				ImagesPaths = productDb.Images.ToPaths()
			};
		}

		public static Product ToProduct(this AddProductViewModel addProductViewModel, List<string> imagesPaths)
		{
			return new Product
			{
				Name = addProductViewModel.Name,
				Description = addProductViewModel.Description,
				Cost = addProductViewModel.Cost,
				Images = imagesPaths.ToImages()
			};
		}

		public static Product ToProduct(this EditProductViewModel editProductViewModel)
		{
			return new Product
			{
				Id = editProductViewModel.Id,
				Name = editProductViewModel.Name,
				Description = editProductViewModel.Description,
				Cost = editProductViewModel.Cost,
				Images = editProductViewModel.ImagesPaths.ToImages()
			};
		}

		private static List<Image> ToImages(this List<string> imagesPaths)
		{
			return imagesPaths.Select(x => new Image { URL = x }).ToList();
		}

		private static List<string> ToPaths(this List<Image> images)
		{
			return images.Select(x => x.URL).ToList();
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
				User = orderDb.User.ToUserDeliveryInfoViewModel(),
				Items = ToCartItemViewModels(orderDb.Items),
				CurrentStatus = (OrderStatusViewModel)(int)orderDb.CurrentStatus,
				Time = orderDb.CreateTime
			};
		}

		public static List<OrderViewModel> ToOrdersViewModel(this List<Order> ordersDb)
		{
			List<OrderViewModel> ordersViewModels = new List<OrderViewModel>();
			foreach (var order in ordersDb)
			{
				OrderViewModel orderViewModel = order.ToOrderViewModel();
				ordersViewModels.Add(orderViewModel);
			}
			return ordersViewModels;
		}

		public static UserDeliveryInfoViewModel ToUserDeliveryInfoViewModel(this UserDeliveryInfo userDeliveryInfoDb)
		{
			return new UserDeliveryInfoViewModel
			{
				Name = userDeliveryInfoDb.Name,
				Address = userDeliveryInfoDb.Address,
				Phone = userDeliveryInfoDb.Phone
			};
		}

		public static UserDeliveryInfo ToUserDeliveryInfo(this UserDeliveryInfoViewModel user, Guid userAccountId)
		{
			return new UserDeliveryInfo
			{
				Name = user.Name,
				Address = user.Address,
				Phone = user.Phone,
				UserAccountId = userAccountId
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
