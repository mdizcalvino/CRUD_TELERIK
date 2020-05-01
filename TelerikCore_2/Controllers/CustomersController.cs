using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using Kendo.Mvc.Extensions;
using Kendo.Mvc.Infrastructure;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Modelos.Contexto;
using Modelos.Modelos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.Dtos;
using TelerikCore_2.Models;
using static WebAPI_2.Controllers.CustomersController;

namespace TelerikCore_2.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomersController(ApplicationDBContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

      


        //public async Task<ActionResult<DataSourceResult>> Get([DataSourceRequest]DataSourceRequest request)
        //{

        //    var a = Request.QueryString;      

        //    var client = _httpClientFactory.CreateClient("TEST");
        //    var response = await client.GetAsync($"Customers{a}").Result.Content.ReadAsStringAsync();

         



        //    var c = JsonConvert.DeserializeObject<DataSourceResult>(response, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
        //    return c;


            
        //    //var job = JObject.Parse(response);

        //    //var c = JsonConvert.DeserializeObject(response, DataSourceResult).ToJson();
           
        //    //DataSourceResult obj = JsonConvert.DeserializeObject<DataSourceResult>(response);

        //    //return Json(response);

        //    //var b = JsonConvert.DeserializeObject<List<Customers>>(response).AsQueryable();
          
           
            
        //    //var result = await myModelList.ToDataSourceResultAsync(request);
        //    //var final = Json(result);


        //    //return Json(obj);

        //}

        public async Task<ActionResult<gridDto<CustomersDto>>> Get([DataSourceRequest]DataSourceRequest request)
        {

            var a = Request.QueryString;

            var client = _httpClientFactory.CreateClient("TEST");
            var response = await client.GetAsync($"Customers{a}").Result.Content.ReadAsStringAsync();


            var resultado = response;


            var c = JsonConvert.DeserializeObject<gridDto<CustomersDto>>(response, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });           

            //var c = JsonConvert.DeserializeObject<DataSourceResult>(response, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

      
            return Json(c);


            //var b = JsonConvert.DeserializeObject<List<Customers>>(response).AsQueryable();
      

        }

        public async Task<ActionResult> Post(CustomersDto customersDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(error => error.ErrorMessage));
            }

            var client = _httpClientFactory.CreateClient("TEST");
            using (var content = new StringContent(JsonConvert.SerializeObject(customersDto), System.Text.Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result = await client.PostAsync("Customers", content);
                if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    return Ok();
                string returnvalue = result.Content.ReadAsStringAsync().Result;
                throw new Exception($"Failed to PUT data : ({result.StatusCode}): {returnvalue}");
            }                   
           

            //return new ObjectResult(new DataSourceResult { Data = new[] { product }, Total = 1 });
        }


        public async Task<ActionResult> Put([DataSourceRequest] DataSourceRequest request, CustomersDto customerdto)
        {

            var id = Request.RouteValues.FirstOrDefault(x => x.Key == "id").Value;

            var client = _httpClientFactory.CreateClient("TEST");
            using (var content = new StringContent(JsonConvert.SerializeObject(customerdto), System.Text.Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result = await client.PutAsync($"Customers/{id}", content);  //.Result; // _httpClient.PostAsync(url, content).Result;
                if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    return Ok();
                string returnValue = result.Content.ReadAsStringAsync().Result;
                throw new Exception($"Failed to POST data: ({result.StatusCode}): {returnValue}");
            }
            //var d = "";
            //var client = _httpClientFactory.CreateClient("TEST");
            //var response = await client.PutAsync("Customers", d).Result.Content.ReadAsStringAsync();
            //var d = "";
            //if (customerdto != null && ModelState.IsValid)
            //{
            //    d = "";
            //}

            //return Json(d);
            //return Json(new[] { product }.ToDataSourceResult(request, ModelState));
        }

        public async Task<ActionResult> Delete(string id)
        {
            
            var client = _httpClientFactory.CreateClient("TEST");
           
            HttpResponseMessage result = await client.DeleteAsync($"Customers/{id}");
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
                return Ok();
            string returnvalue = result.Content.ReadAsStringAsync().Result;
            throw new Exception($"Failed to PUT data : ({result.StatusCode}): {returnvalue}");
            


            //return new ObjectResult(new DataSourceResult { Data = new[] { product }, Total = 1 });
        }



        //public ActionResult Orders_Read([DataSourceRequest]DataSourceRequest request)
        //{
        //    var result = Enumerable.Range(0, 50).Select(i => new OrderViewModel
        //    {
        //        OrderID = i,
        //        Freight = i * 10,
        //        OrderDate = new DateTime(2016, 9, 15).AddDays(i % 7),
        //        ShipName = "ShipName " + i,
        //        ShipCity = "ShipCity " + i
        //    });

        //    var dsResult = result.ToDataSourceResult(request);
        //    return Json(dsResult);
        //}

        //public ActionResult Get()
        //{
        //    return View();
        //}
    }
}