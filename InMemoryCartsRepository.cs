using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
	public class InMemoryCartsRepository : ICartsRepository
	{
		private List<Cart> carts = new List<Cart>();

		public Cart TryGetByUserId(string userId)
		{
			return carts.FirstOrDefault(cart => cart.UserId == userId);
		}

		public void Add(Product product, string UserId)
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
