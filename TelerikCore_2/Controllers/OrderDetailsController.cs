using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Dtos;

namespace TelerikCore_2.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderDetailsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ActionResult<gridDto<OrderDetailsDto>>> GetDetailOrder([DataSourceRequest]DataSourceRequest request, string id)
        {

            //if(request.PageSize == 0) { request.PageSize = 10; }

            var a = Request.QueryString;

            var client = _httpClientFactory.CreateClient("TEST");
            var response = await client.GetAsync($"OrderDetails/{id}{a}").Result.Content.ReadAsStringAsync();


            var c = JsonConvert.DeserializeObject<gridDto<OrderDetailsDto>>(response, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });


            // prueba

            return Json(c);

        }
    }
}