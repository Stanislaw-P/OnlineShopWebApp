using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Onlineshop.Db.Models;
using OnlineShop.Db;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
    public class ProductController : Controller
    {
		readonly IProductsRepository productsRepitory;
        readonly IMapper _mapper;
        readonly IMemoryCache cache;

		public ProductController(IProductsRepository productsRepository, IMapper mapper, IMemoryCache cache)
		{
			productsRepitory = productsRepository;
			_mapper = mapper;
			this.cache = cache;
		}

		public IActionResult Index(Guid productId)
        {
            cache.TryGetValue<Product>(productId, out var product);
            if (product == null)
                return NotFound();
            return View(_mapper.Map<ProductViewModel>(product));
        }
    }
}
