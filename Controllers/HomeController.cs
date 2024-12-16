using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;
using System.Diagnostics;

namespace OnlineShopWebApp.Controllers
{
    public class CalculatorController : Controller
    {
        public string Index(int a, int b, char c = '+')
        {
            if (c == '-') return $"{a} {c} {b} = {a-b}";
            if (c == '*') return $"{a} {c} {b} = {a * b}";
            if (c == '/') return $"{a} {c} {b} = {a / b}";
            if (c == '+') return $"{a} {c} {b} = {a + b}";
            return "Данная операция недоступна.\nСущетсвует возможность только ввода '+', '-' и '*', '/'";
        }
    }

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}