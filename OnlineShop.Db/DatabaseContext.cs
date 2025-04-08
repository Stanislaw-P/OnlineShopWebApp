using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Onlineshop.Db.Models;
using OnlineShop.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Db
{
	public class DatabaseContext : DbContext
	{
		public DbSet<Product> Products { get; set; }
		public DbSet<Cart> Carts { get; set; }
		public DbSet<FavoriteProduct> FavoriteProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
		public DbSet<Image> Images { get; set; }
		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
		{
			Database.Migrate();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Image>().HasOne(p => p.Product).WithMany(p => p.Images).HasForeignKey(p => p.ProductId).OnDelete(DeleteBehavior.Cascade);
			var product1Id = Guid.Parse("84bc6770-71d5-4216-b98f-e40a676fc5a7");
			var product2Id = Guid.Parse("39d324be-60dc-4f27-8be7-1fae55ae45f7");

			var image1 = new Image
			{
				Id = Guid.Parse("9609bc38-1ec6-4b59-adc8-0f65bb19a967"),
				URL = "/images/f869fff1-89b9-4e2e-bfed-6002a76e939d.jpg",
				ProductId = product1Id
			};

			var image2 = new Image
			{
				Id = Guid.Parse("a53bf6dd-c64a-4895-ba03-301ee7c12a51"),
				URL = "/images/f100232c-760a-458f-8f84-395489757aa4.webp",
				ProductId = product2Id
			};

			modelBuilder.Entity<Image>().HasData(image1, image2);

			modelBuilder.Entity<Product>().HasData(new List<Product>()
			{
				new Product(product1Id, "Product1", 100, "Desc1"),
				new Product(product2Id, "Product2", 200, "Desc2"),
			});
		}
	}
}
