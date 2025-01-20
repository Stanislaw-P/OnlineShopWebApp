using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
	public static class CartsRepository
	{
		private static List<Cart> carts = new List<Cart>();

		public static Cart TryGetByUserId(string userId)
		{
			return carts.FirstOrDefault(cart => cart.UserId == userId);
		}

		internal static void Add(Product product, string UserId)
		{
			Cart existingCart = TryGetByUserId(UserId);
			if (existingCart == null)
			{
				Cart newCart = new Cart()
				{
					Id = Guid.NewGuid(),
					UserId = UserId,
					Items = new List<CartItem>
					{
						new CartItem
						{
							Id = Guid.NewGuid(),
							Product = product,
							Amount = 1
						}
					}
				};
				carts.Add(newCart);
			}
			else
			{
				CartItem existingCartItem = existingCart.Items.FirstOrDefault(cartItem => cartItem.Product.Id == product.Id);
				if (existingCartItem != null)
				{
					existingCartItem.Amount += 1;
				}
				else
				{
					CartItem newCartItem = new CartItem()
					{
						Id = Guid.NewGuid(),
						Product = product,
						Amount = 1
					};
					existingCart.Items.Add(newCartItem);
				}
			}
		}
	}
}
