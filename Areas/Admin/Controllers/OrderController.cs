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

		public OrderController(IOrdersRepository ordersRepository)
		{
			this.ordersRepository = ordersRepository;
		}

		public IActionResult Index()
		{
			var orders = ordersRepository.GetAll();
			return View(orders.Select(order => order.ToOrderViewModel()).ToList());
		}

		public IActionResult Details(Guid orderId)
		{
			Order? existingOrder = ordersRepository.TryGetById(orderId);
			OrderViewModel orderViewModel = existingOrder.ToOrderViewModel();
			if (orderViewModel == null)
				return NotFound();
			ViewBag.OrderNumber = ordersRepository.GetAll().IndexOf(existingOrder) + 1;
			return View(orderViewModel);
		}

		public IActionResult UpdateOrderStatus(Guid orderId, OrderStatusViewModel newStatus)
		{
			ordersRepository.UpdateStatus(orderId, (OrderStatus)(int)newStatus);
			return RedirectToAction(nameof(Index));
		}
	}
}
