using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Dtos;

namespace TelerikCore_2.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> OrdersView ()
        {
            var client = _httpClientFactory.CreateClient("TEST");
            var response = await client.GetAsync($"Customers/Cbo").Result.Content.ReadAsStringAsync();
            var customerCboDto = JsonConvert.DeserializeObject<List<CustomersCboDto>>(response, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

            ViewData["customers"] = customerCboDto;   //.to categories.ToList();
            ViewData["defaultCustomer"] = customerCboDto.First();

            response = await client.GetAsync($"Employees/Cbo").Result.Content.ReadAsStringAsync();
            var employeeCboDto = JsonConvert.DeserializeObject<List<EmployeCboDto>>(response, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

            ViewData["employees"] = employeeCboDto;   //.to categories.ToList();
            ViewData["defaultEmployee"] = employeeCboDto.First();
            //ViewData["defaultCategory"] = categories.First();

            response = await client.GetAsync($"Products/Cbo").Result.Content.ReadAsStringAsync();
            var productsCboDto = JsonConvert.DeserializeObject<List<ProductCboDto>>(response, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

            ViewData["products"] = productsCboDto;   //.to categories.ToList();
            ViewData["defaultProduct"] = productsCboDto.First();

            return View();
        }

        public async Task<IActionResult> ProductsView()
        {
            var client = _httpClientFactory.CreateClient("TEST");
            var response = await client.GetAsync($"Categories").Result.Content.ReadAsStringAsync();
            var categoryDto = JsonConvert.DeserializeObject<List<CategoryDto>>(response, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

            ViewData["categories"] = categoryDto;   //.to categories.ToList();
            ViewData["defaultCategory"] = categoryDto.First();
            //ViewData["defaultCategory"] = categories.First();

            return View();
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
