using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Onlineshop.Db.Models;
using OnlineShop.Db;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
    public class HomeController : Controller
    {
        readonly IMapper mapper;
        readonly IMemoryCache cache;
		public HomeController(IMapper mapper, IMemoryCache cache)
		{
			this.mapper = mapper;
			this.cache = cache;
		}

		public IActionResult Index()
        {
            cache.TryGetValue<List<Product>>(Constants.KeyCacheAllProducts, out var productsDb);
            var productsModels = mapper.Map<List<ProductViewModel>>(productsDb);
            return View(productsModels);
        }
    }
}