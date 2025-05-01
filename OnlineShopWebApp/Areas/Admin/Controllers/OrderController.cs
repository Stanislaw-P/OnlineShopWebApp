using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Models;
using System.Reflection.Metadata;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
	[Area(Constants.AdminRoleName)]
	[Authorize(Roles = Constants.AdminRoleName)]
	public class OrderController : Controller
	{
		readonly IOrdersRepository ordersRepository;
		readonly IMapper _mapper;

		public OrderController(IOrdersRepository ordersRepository, IMapper mapper)
		{
			this.ordersRepository = ordersRepository;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index()
		{
			var orders = await ordersRepository.GetAllAsync();
			return View(_mapper.Map<List<OrderViewModel>>(orders));
		}

		public async Task<IActionResult> DetailsAsync(Guid orderId)
		{
			Order? existingOrder = await ordersRepository.TryGetByIdAsync(orderId);
			if (existingOrder == null)
				return NotFound();
			OrderViewModel orderViewModel = _mapper.Map<OrderViewModel>(existingOrder);
			
			var orders = await ordersRepository.GetAllAsync();
			ViewBag.OrderNumber = orders.IndexOf(existingOrder) + 1;
			return View(orderViewModel);
		}

		public async Task<IActionResult> UpdateOrderStatusAsync(Guid orderId, OrderStatusViewModel newStatus)
		{
			await ordersRepository.UpdateStatusAsync(orderId, (OrderStatus)(int)newStatus);
			return RedirectToAction(nameof(Index));
		}
	}
}
