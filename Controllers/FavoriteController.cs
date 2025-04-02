using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
	[Authorize]
    public class FavoriteController : Controller
	{
		readonly IFavoriteRepository favoritesRepository;
		readonly IProductsRepository productsRepository;
		readonly IMapper _mapper;

		public FavoriteController(IFavoriteRepository favoritesRepository, IProductsRepository productsRepository, IMapper mapper)
		{
			this.favoritesRepository = favoritesRepository;
			this.productsRepository = productsRepository;
			_mapper = mapper;
		}

		public IActionResult Index()
		{
			var products = favoritesRepository.GetAll(Constants.UserId);
			return View(_mapper.Map<List<ProductViewModel>>(products));
		}

		public IActionResult Add(Guid productId)
		{
			var product = productsRepository.TryGetById(productId);
			favoritesRepository.Add(product, Constants.UserId);
			return RedirectToAction(nameof(Index), "Home");
		}

		public IActionResult RemoveProduct(Guid productId)
		{
			var product = productsRepository.TryGetById(productId);
			favoritesRepository.Remove(product, Constants.UserId);
			return RedirectToAction(nameof(Index));
		}
	}
}
