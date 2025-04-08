
using Microsoft.EntityFrameworkCore;
using Onlineshop.Db.Models;
using OnlineShop.Db.Models;

namespace OnlineShop.Db
{
	public class CartsDbRepository : ICartsRepository
	{
		readonly DatabaseContext databaseContext;

		public CartsDbRepository(DatabaseContext databaseContext)
		{
			this.databaseContext = databaseContext;
		}

		public Cart TryGetByUserId(string userId)
		{
			return databaseContext.Carts.Include(x => x.Items).ThenInclude(x => x.Product).FirstOrDefault(cart => cart.UserId == userId);
		}

		public void Add(Product product, string userId)
		{
			Cart existingCart = TryGetByUserId(userId);
			if (existingCart == null)
			{
				Cart newCart = new Cart()
				{
					UserId = userId,
				};

				newCart.Items = new List<CartItem>
					{
						new CartItem
						{
							Product = product,
							Amount = 1,
						}
					};
				databaseContext.Carts.Add(newCart);
			}
			else
			{
				CartItem existingCartItem = existingCart.Items.FirstOrDefault(cartItem => cartItem.Product.Id == product.Id);
				if (existingCartItem != null)
				{
					existingCartItem.Amount++;
				}
				else
				{
					CartItem newCartItem = new CartItem()
					{
						Product = product,
						Amount = 1,					
					};
					existingCart.Items.Add(newCartItem);
				}
			}
			databaseContext.SaveChanges();
		}

		public void DecreaseAmount(Product product, string userId)
		{
			Cart existingCart = TryGetByUserId(userId);
			CartItem? existingCartItem = existingCart?.Items?.FirstOrDefault(cartItem => cartItem.Product.Id == product.Id);
			if (existingCartItem == null)
			{
				return;
			}
			existingCartItem.Amount--;
			if (existingCartItem.Amount == 0)
				existingCart?.Items.Remove(existingCartItem);
			databaseContext.SaveChanges();
		}

		public void IcreaseAmount(Product product, string userId)
		{
			Cart existingCart = TryGetByUserId(userId);
			CartItem? existingCartItem = existingCart?.Items?.FirstOrDefault(cartItem => cartItem.Product.Id == product.Id);
			if (existingCartItem == null)
				return;
			existingCartItem.Amount++;
			databaseContext.SaveChanges();
		}

		public void ClearCartByUserId(string userId)
		{
			Cart existingCart = TryGetByUserId(userId);
			databaseContext.Carts.Remove(existingCart);
			databaseContext.SaveChanges();
		}
	}
}
