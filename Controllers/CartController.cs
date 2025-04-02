using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        readonly IMapper _mapper;
		public CartController(IProductsRepository productsRepository, ICartsRepository cartsRepository, IMapper mapper)
		{
			this.productsRepository = productsRepository;
			this.cartsRepository = cartsRepository;
			_mapper = mapper;
		}

		public IActionResult Index()
        {
            Cart cartDb = cartsRepository.TryGetByUserId(Constants.UserId);
            CartViewModel cartViewModel = _mapper.Map<CartViewModel>(cartDb);
            return View(cartViewModel);
        }
        
        public IActionResult Add(Guid productId)
        {
            var product = productsRepository.TryGetById(productId);
            cartsRepository.Add(product, Constants.UserId);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult IncreaseAmount(Guid productId)
        {
            var product = productsRepository.TryGetById(productId);
            cartsRepository.IcreaseAmount(product, Constants.UserId);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DecreaseAmount(Guid productId)
        {
            var product = productsRepository.TryGetById(productId);
            cartsRepository.DecreaseAmount(product, Constants.UserId);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Clear()
        {
            cartsRepository.ClearCartByUserId(Constants.UserId);
            return RedirectToAction(nameof(Index));
		}
	}
}
