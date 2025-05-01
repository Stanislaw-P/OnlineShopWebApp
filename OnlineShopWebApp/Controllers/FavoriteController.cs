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
	public class FavoriteController : Controller
	{
		readonly IFavoriteRepository favoritesRepository;
		readonly IProductsRepository productsRepository;
		readonly UserManager<User> _userManager;
		readonly IMapper _mapper;

		public FavoriteController(IFavoriteRepository favoritesRepository, IProductsRepository productsRepository, IMapper mapper, UserManager<User> userManager)
		{
			this.favoritesRepository = favoritesRepository;
			this.productsRepository = productsRepository;
			_mapper = mapper;
			_userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			var currentUserId = _userManager.GetUserId(User);
			if (currentUserId == null)
				return Unauthorized();
			var products = await favoritesRepository.GetAllAsync(currentUserId);
			return View(_mapper.Map<List<ProductViewModel>>(products));
		}

		public async Task<IActionResult> AddAsync(Guid productId)
		{
			var product = await productsRepository.TryGetByIdAsync(productId);
			var currentUserId = _userManager.GetUserId(User);

			if (currentUserId == null)
				return Unauthorized();
			if (product == null)
				return NotFound();

			await favoritesRepository.Addsync(product, currentUserId);
			return RedirectToAction(nameof(Index), "Home");
		}

		public async Task<IActionResult> RemoveProductAsync(Guid productId)
		{
			var product = await productsRepository.TryGetByIdAsync(productId);
			var currentUserId = _userManager.GetUserId(User);
			if (currentUserId == null)
				return Unauthorized();
			if (product == null)
				return NotFound();
			await favoritesRepository.RemoveAsync(product, currentUserId);
			return RedirectToAction(nameof(Index));
		}
	}
}
