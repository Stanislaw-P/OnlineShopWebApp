using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Models;
using System.Diagnostics;

namespace OnlineShopWebApp.Controllers
{
    public class HomeController : Controller
    {
        readonly IProductsRepository productsRepository;
        readonly IMapper mapper;
		public HomeController(IProductsRepository productsRepository, IMapper mapper)
		{
			this.productsRepository = productsRepository;
			this.mapper = mapper;
		}

		public IActionResult Index()
        {
            var productsDb = productsRepository.GetAll();
            var productsModels = mapper.Map<List<ProductViewModel>>(productsDb);
			return View(productsModels);
        }
    }
}