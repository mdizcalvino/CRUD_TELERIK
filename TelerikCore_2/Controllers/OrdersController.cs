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
    public class OrdersController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public OrdersController(IHttpClientFactory httpClientFactory )
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ActionResult<gridDto<OrderDto>>> Get([DataSourceRequest]DataSourceRequest request)
        {

            var a = Request.QueryString;

            var client = _httpClientFactory.CreateClient("TEST");
            var response = await client.GetAsync($"Orders{a}").Result.Content.ReadAsStringAsync();            


            var c = JsonConvert.DeserializeObject<gridDto<OrderDto>>(response, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });         


            return Json(c);           


        }

        public async Task<ActionResult> Put([DataSourceRequest] DataSourceRequest request, OrderDto orderdto)
        {

            var id = Request.RouteValues.FirstOrDefault(x => x.Key == "id").Value;

            var client = _httpClientFactory.CreateClient("TEST");
            using (var content = new StringContent(JsonConvert.SerializeObject(orderdto), System.Text.Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result = await client.PutAsync($"Orders/{id}", content);  //.Result; // _httpClient.PostAsync(url, content).Result;
                if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    return Ok();
                string returnValue = result.Content.ReadAsStringAsync().Result;
                throw new Exception($"Failed to POST data: ({result.StatusCode}): {returnValue}");
            }
           
        }
    }
}