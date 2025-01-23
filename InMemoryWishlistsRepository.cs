﻿using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
	public class InMemoryWishlistsRepository : IWishlistsRepository
	{
		private List<Wishlist> wishlists = new List<Wishlist>();

		public Wishlist? TryGetByUserId(string userId)
		{
			return wishlists.FirstOrDefault(wishlist => wishlist.UserId == userId);
		}

		public void Add(Product product, string userId)
		{
			Wishlist? existingWishlist = TryGetByUserId(userId);
			if (existingWishlist == null)
			{
				Wishlist newWishlist1 = new Wishlist()
				{
					ID = Guid.NewGuid(),
					UserId = userId,
					Items = new List<Product>
					{
						product
					}
				};
				wishlists.Add(newWishlist1);
			}
			else
			{
				Product? existingProduct = existingWishlist.Items.FirstOrDefault(pr => pr.Id == product.Id);
				if (existingProduct == null)
					existingWishlist.Items.Add(product);
			}
		}

		public void RemoveProductByUserId(Product product, string userId)
		{
			Wishlist? existingWishlist = TryGetByUserId(userId);
			if (existingWishlist == null)
				return;
			existingWishlist.Items.Remove(product);
		}
	}
}
