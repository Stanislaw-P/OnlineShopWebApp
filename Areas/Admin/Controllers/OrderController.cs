using Microsoft.AspNetCore.Mvc;
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
            List<Order> orders = ordersRepository.GetAll();
            return View(orders);
        }

        public IActionResult Detail(Guid orderId)
        {
            Order? existingOrder = ordersRepository.TryGetById(orderId);
            if (existingOrder == null)
                return NotFound();
            ViewBag.OrderNumber = ordersRepository.GetAll().IndexOf(existingOrder) + 1;
            return View(existingOrder);
        }

        public IActionResult UpdateOrderStatus(Guid orderId, OrderStatus newStatus)
        {
			ordersRepository.UpdateStatus(orderId, newStatus);
            return RedirectToAction(nameof(Index));
        }
    }
}
