using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Dtos;
using Services.HttpServices;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using IdentityServer4.AccessTokenValidation;

namespace TelerikCore_2.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory )
        {
            _httpClientFactory = httpClientFactory;

        }

        
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return new SignOutResult(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = Url.Action(nameof(LogoutSuccess)) });
        }

        [HttpGet("logoutSuccess")]
        public IActionResult LogoutSuccess() => View("about");


        //public IActionResult LogOut()
        //{

        //    return new SignOutResult(new List<string> { CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme });


        //    //await _signInManager.SignOutAsync();
        //    // await HttpContext.SignOutAsync(IdentityserverConstants  "Identity.Application");
        //    //await HttpContext.SignOutAsync();
        //    //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    //await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

        //    //return Redirect("/"); 

        //    //return LocalRedirect($"/Home/About");
        //    //return View("About");
        //}

        //public IActionResult LogOutRedirect()
        //{
        //    return View("/home/about");
        //}


        [Authorize(Policy = "RequireAdministratorRole")]
        public IActionResult Index()
        {
            return View();
        }




        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> OrdersView ()
        {
            IGenericHttpService<OrderDto> servicio = HttpContext.RequestServices.GetService<IGenericHttpService<OrderDto>>();
            servicio.cliente = "TEST";
            servicio.controlador = "Orders";

            var kv = await servicio.HttpCbosAsync();


            kv.Value.ForEach(x => {                

                switch (x.Key)
                {
                    case "customers" :                        
                        ViewData[x.Key] = ((JArray)x.Value).ToObject(typeof(List<CustomersCboDto>)); 
                        ViewData["defaultCustomer"] =((List<CustomersCboDto>)((JArray)x.Value).ToObject(typeof(List<CustomersCboDto>))).First();
                        break;
                    case "employees":
                        ViewData[x.Key] = ((JArray)x.Value).ToObject(typeof(List<EmployeCboDto>)); 
                        ViewData["defaultEmployee"] = ((List<EmployeCboDto>)((JArray)x.Value).ToObject(typeof(List<EmployeCboDto>))).First();
                        break;
                    case "products":
                        ViewData[x.Key] = ((JArray)x.Value).ToObject(typeof(List<ProductCboDto>)); 
                        ViewData["defaultProduct"] = ((List<ProductCboDto>)((JArray)x.Value).ToObject(typeof(List<ProductCboDto>))).First();
                        break;
                    default:
                        break;
                }          

           });

            //Type type = Assembly.GetCallingAssembly.GetType("")

            //KeyValuePair<HttpStatusCode, List<KeyValuePair<string, object>>> objetos = JsonConvert.DeserializeObject<KeyValuePair<HttpStatusCode, List<KeyValuePair<string, object>>>>(kv, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

            //var a = "";

            //var client = _httpClientFactory.CreateClient("TEST");
            //var response = await client.GetAsync($"Customers/Cbo").Result.Content.ReadAsStringAsync();
            //var customerCboDto = JsonConvert.DeserializeObject<List<CustomersCboDto>>(response, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

            //ViewData["customers"] = customerCboDto;   //.to categories.ToList();
            //ViewData["defaultCustomer"] = customerCboDto.First();

            //response = await client.GetAsync($"Employees/Cbo").Result.Content.ReadAsStringAsync();
            //var employeeCboDto = JsonConvert.DeserializeObject<List<EmployeCboDto>>(response, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

            //ViewData["employees"] = employeeCboDto;   //.to categories.ToList();
            //ViewData["defaultEmployee"] = employeeCboDto.First();
            ////ViewData["defaultCategory"] = categories.First();

            //response = await client.GetAsync($"Products/Cbo").Result.Content.ReadAsStringAsync();
            //var productsCboDto = JsonConvert.DeserializeObject<List<ProductCboDto>>(response, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

            //ViewData["products"] = productsCboDto;   //.to categories.ToList();
            //ViewData["defaultProduct"] = productsCboDto.First();

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
