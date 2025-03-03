using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            return View(orders.Select(order => MappingHelper.ToOrderViewModel(order)).ToList());
        }

        public IActionResult Details(Guid orderId)
        {
            Order? existingOrder = ordersRepository.TryGetById(orderId);
            OrderViewModel orderViewModel = MappingHelper.ToOrderViewModel(existingOrder);
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
