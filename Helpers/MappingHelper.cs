using Onlineshop.Db.Models;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Helpers
{
	public static class MappingHelper
	{
		public static List<ProductViewModel> ToProductViewModels(List<Product> productsDb)
		{
			List<ProductViewModel> productsViewModels = new List<ProductViewModel>();
			foreach (var productDb in productsDb)
			{
				ProductViewModel productViewModel = ToProductViewModel(productDb);

				productsViewModels.Add(productViewModel);
			}
			return productsViewModels;
		}

		public static ProductViewModel ToProductViewModel(Product productDb)
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

		public static CartViewModel ToCartViewModel(Cart cartDb)
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

		public static List<CartItemViewModel> ToCartItemViewModels(List<CartItem> cartDtItems)
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
	}
}
