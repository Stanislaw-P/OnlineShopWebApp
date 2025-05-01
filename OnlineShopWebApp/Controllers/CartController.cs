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

		public async Task<IActionResult> Index()
        {
            var currentUserId = _userManager.GetUserId(User);
            if (currentUserId == null)
                return Unauthorized();

            Cart? cartDb = await cartsRepository.TryGetByUserIdAsync(currentUserId);
            CartViewModel cartViewModel = _mapper.Map<CartViewModel>(cartDb);
            return View(cartViewModel);
        }
        
        public async Task<IActionResult> AddAsync(Guid productId)
        {
            var product = await productsRepository.TryGetByIdAsync(productId);
            var currentUserId = _userManager.GetUserId(User);

            if (product == null)
                return NotFound();
            if (currentUserId == null)
                return Unauthorized();

			await cartsRepository.AddAsync(product, currentUserId);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> IncreaseAmountAsync(Guid productId)
        {
            var product = await productsRepository.TryGetByIdAsync(productId);
			var currentUserId = _userManager.GetUserId(User);
			if (currentUserId == null)
				return Unauthorized();
            if (product == null)
                return NotFound();
			await cartsRepository.IcreaseAmounAsync(product, currentUserId);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DecreaseAmountAsync(Guid productId)
        {
            var product = await productsRepository.TryGetByIdAsync(productId);
			var currentUserId = _userManager.GetUserId(User);

			if (currentUserId == null)
				return Unauthorized();
            if (product == null)
                return NotFound();

			await cartsRepository.DecreaseAmountAsync(product, currentUserId);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ClearAsync()
        {
			var currentUserId = _userManager.GetUserId(User);
			if (currentUserId == null)
				return Unauthorized();

			await cartsRepository.ClearCartByUserIdAsync(currentUserId);
            return RedirectToAction(nameof(Index));
		}
	}
}
