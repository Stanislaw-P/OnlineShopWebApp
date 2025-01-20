using Microsoft.AspNetCore.Mvc;

namespace OnlineShopWebApp.Controllers
{
	public class RandomController : Controller
	{
		private readonly ICounter randomCounter;
		private readonly CounterService counterService;

		public RandomController(ICounter randomCounter, CounterService counterService)
		{
			this.randomCounter = randomCounter;
			this.counterService = counterService;
		}


		public string Index()
		{
			return $"RandomCounter = {randomCounter.Value}\nCounterService = {counterService.Counter.Value}";
		}
	}
}
