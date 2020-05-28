using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Dtos;
using Services.HttpServices;

namespace TelerikCore_2.Controllers
{

    
    [Authorize(Policy = "RequireAdministratorRole")]    
    public class OrderDetailsController : Controller, IGenericController<OrderDetailsDto>
    {
        private readonly IGenericHttpService<OrderDetailsDto> _genericHttpService;

        public OrderDetailsController(IGenericHttpService<OrderDetailsDto> genericHttpService)
        {
            _genericHttpService = genericHttpService;
            _genericHttpService.controlador = "OrderDetails";
            _genericHttpService.cliente = "TEST";

        }


        public Task<ActionResult> Delete(OrderDetailsDto entityDto, string id)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<gridDto<OrderDetailsDto>>> Get([DataSourceRequest] DataSourceRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult<gridDto<OrderDetailsDto>>> GetDetail([DataSourceRequest]DataSourceRequest request, string id)
        {

            var query = Request.QueryString;
            var (sc, _gridDto) = await _genericHttpService.HttpGetDetailAsync(query, id);

            return StatusCode((int)sc, _gridDto);

            //var a = Request.QueryString;

            //var client = _httpClientFactory.CreateClient("TEST");
            //var response = await client.GetAsync($"OrderDetails/{id}{a}").Result.Content.ReadAsStringAsync();


            //var c = JsonConvert.DeserializeObject<gridDto<OrderDetailsDto>>(response, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });


            //// prueba

            //return Json(c);

        }

        public Task<ActionResult> Post(OrderDetailsDto entityDto)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> Put(OrderDetailsDto entityDto, string id)
        {
            throw new NotImplementedException();
        }
    }
}