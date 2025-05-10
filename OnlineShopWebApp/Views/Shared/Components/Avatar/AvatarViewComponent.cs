using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Views.Shared.Components.Avatar
{
	public class AvatarViewComponent : ViewComponent
	{
		readonly UserManager<User> _userManager;
		readonly IMemoryCache cache;
		readonly IMapper _mapper;

		public AvatarViewComponent(UserManager<User> userManager, IMemoryCache cache, IMapper mapper)
		{
			_userManager = userManager;
			this.cache = cache;
			_mapper = mapper;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var currentUserId = _userManager.GetUserId(HttpContext.User);
			var currentUser = await _userManager.Users
				.Include(us => us.Avatar)
				.FirstOrDefaultAsync(us => us.Id == currentUserId);

			if (currentUser != null)
			{
				AvatarImageViewModel avatarImage = null;
				if (!cache.TryGetValue(currentUserId, out avatarImage))
				{
					// Получаем объект из БД
					avatarImage = _mapper.Map<AvatarImageViewModel>(currentUser);
					if (avatarImage != null)
					{
						// Добавляем в кеш на 15 мин
						cache.Set(currentUserId, avatarImage,
							new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(15)));
					}
				}
				return View("Avatar", avatarImage);
			}
			return null;
		}
	}
}
