
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

		public async Task<Cart?> TryGetByUserIdAsync(string userId)
		{
			return await databaseContext.Carts
				.Include(x => x.Items)
				.ThenInclude(x => x.Product)
				.FirstOrDefaultAsync(cart => cart.UserId == userId);
		}

		public async Task AddAsync(Product product, string userId)
		{
			Cart? existingCart = await TryGetByUserIdAsync(userId);
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
				await databaseContext.Carts.AddAsync(newCart);
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
			await databaseContext.SaveChangesAsync();
		}

		public async Task DecreaseAmountAsync(Product product, string userId)
		{
			Cart? existingCart = await TryGetByUserIdAsync(userId);
			CartItem? existingCartItem = existingCart?.Items.FirstOrDefault(cartItem => cartItem.Product.Id == product.Id);
			if (existingCartItem == null)
			{
				return;
			}
			existingCartItem.Amount--;
			if (existingCartItem.Amount == 0)
				existingCart?.Items.Remove(existingCartItem);

			await databaseContext.SaveChangesAsync();
		}

		public async Task IcreaseAmounAsync(Product product, string userId)
		{
			Cart? existingCart = await TryGetByUserIdAsync(userId);
			CartItem? existingCartItem = existingCart?.Items.FirstOrDefault(cartItem => cartItem.Product.Id == product.Id);
			if (existingCartItem == null)
				return;
			existingCartItem.Amount++;
			await databaseContext.SaveChangesAsync();
		}

		public async Task ClearCartByUserIdAsync(string userId)
		{
			Cart? existingCart = await TryGetByUserIdAsync(userId);
			if (existingCart != null)
			{
				databaseContext.Carts.Remove(existingCart);
				await databaseContext.SaveChangesAsync();
			}
		}
	}
}
