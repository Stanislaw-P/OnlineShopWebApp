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

		public IActionResult Index()
		{
			var currentUserId = _userManager.GetUserId(User);
			if (currentUserId == null)
				return Unauthorized();
			var products = favoritesRepository.GetAll(currentUserId);
			return View(_mapper.Map<List<ProductViewModel>>(products));
		}

		public IActionResult Add(Guid productId)
		{
			var product = productsRepository.TryGetById(productId);
			var currentUserId = _userManager.GetUserId(User);
			if (currentUserId == null)
				return Unauthorized();
			favoritesRepository.Add(product, currentUserId);
			return RedirectToAction(nameof(Index), "Home");
		}

		public IActionResult RemoveProduct(Guid productId)
		{
			var product = productsRepository.TryGetById(productId);
			var currentUserId = _userManager.GetUserId(User);
			if (currentUserId == null)
				return Unauthorized();
			favoritesRepository.Remove(product, currentUserId);
			return RedirectToAction(nameof(Index));
		}
	}
}
