using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Db.Models;
using System;

namespace OnlineShop.Db
{
	public class IdentityContext : IdentityDbContext<User>
	{
		public DbSet<AvatarImage> AvatarImages { get; set; }
		public IdentityContext(DbContextOptions<IdentityContext> options) 
			: base(options)
		{
			Database.Migrate();
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<User>()
				.HasOne(us => us.Avatar)
				.WithOne(av => av.User)
				.HasForeignKey<AvatarImage>(av => av.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<AvatarImage>()
				.Property(av => av.UserId)
				.IsRequired();
		}
	} 
}
