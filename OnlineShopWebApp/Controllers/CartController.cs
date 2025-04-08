using AutoMapper;
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
    public class CartController : Controller
    {
		readonly IProductsRepository productsRepository;
        readonly ICartsRepository cartsRepository;
        readonly UserManager<User> _userManager;
        readonly IMapper _mapper;
		public CartController(IProductsRepository productsRepository, ICartsRepository cartsRepository, IMapper mapper, UserManager<User> userManager)
		{
			this.productsRepository = productsRepository;
			this.cartsRepository = cartsRepository;
			_mapper = mapper;
			_userManager = userManager;
		}

		public IActionResult Index()
        {
            var currentUserId = _userManager.GetUserId(User);
            if (currentUserId == null)
                return Unauthorized();

            Cart cartDb = cartsRepository.TryGetByUserId(currentUserId);
            CartViewModel cartViewModel = _mapper.Map<CartViewModel>(cartDb);
            return View(cartViewModel);
        }
        
        public IActionResult Add(Guid productId)
        {
            var product = productsRepository.TryGetById(productId);
            var currentUserId = _userManager.GetUserId(User);

			cartsRepository.Add(product, currentUserId);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult IncreaseAmount(Guid productId)
        {
            var product = productsRepository.TryGetById(productId);
			var currentUserId = _userManager.GetUserId(User);
			if (currentUserId == null)
				return Unauthorized();
			cartsRepository.IcreaseAmount(product, currentUserId);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DecreaseAmount(Guid productId)
        {
            var product = productsRepository.TryGetById(productId);
			var currentUserId = _userManager.GetUserId(User);
			if (currentUserId == null)
				return Unauthorized();
			cartsRepository.DecreaseAmount(product, currentUserId);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Clear()
        {
			var currentUserId = _userManager.GetUserId(User);
			if (currentUserId == null)
				return Unauthorized();

			cartsRepository.ClearCartByUserId(currentUserId);
            return RedirectToAction(nameof(Index));
		}
	}
}
