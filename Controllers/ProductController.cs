using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
    public class ProductController : Controller
    {
		IProductsRepository productsRepitory;
        readonly IMapper _mapper;

		public ProductController(IProductsRepository productsRepository, IMapper mapper)
		{
			productsRepitory = productsRepository;
			_mapper = mapper;
		}

		public IActionResult Index(Guid productId)
        {
            var product = productsRepitory.TryGetById(productId);
            if (product == null)
                return NotFound();
            return View(_mapper.Map<ProductViewModel>(product));
        }
    }
}
