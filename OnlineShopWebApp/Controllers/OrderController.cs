﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
	[Authorize]
	public class OrderController : Controller
	{
		readonly ICartsRepository cartsRepository;
		readonly IOrdersRepository ordersRepository;
		readonly UserManager<User> userManager;
		readonly IMapper _mapper;

		public OrderController(ICartsRepository cartsRepository, IOrdersRepository ordersRepository, UserManager<User> userManager, IMapper mapper)
		{
			this.cartsRepository = cartsRepository;
			this.ordersRepository = ordersRepository;
			this.userManager = userManager;
			_mapper = mapper;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> BuyAsync(UserDeliveryInfoViewModel userViewModel)
		{
			if (!ModelState.IsValid)
				return View("Index", userViewModel);

			var currentUser = await userManager.GetUserAsync(HttpContext.User);
			if (currentUser == null)
				return Unauthorized();

			Guid idCurrentUser = Guid.Parse(currentUser.Id);

			Cart? existingCartDb = await cartsRepository.TryGetByUserIdAsync(currentUser.Id);
			Order order = new Order
			{
				// Тут наверно не хватает других свойств. Не знаю под чем я этот бред писал...
				User = _mapper.Map<UserDeliveryInfo>(userViewModel, opt => opt.Items["UserAccountId"] = idCurrentUser),				
				Items = existingCartDb.Items,
			};

			ordersRepository.AddAsync(order);

			await cartsRepository.ClearCartByUserIdAsync(currentUser.Id);
			return View();
		}
	}
}
