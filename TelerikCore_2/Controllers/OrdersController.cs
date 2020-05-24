﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Dtos;
using Services.HttpServices;

namespace TelerikCore_2.Controllers
{

    [Authorize(Policy = "RequireAdministratorRole")]
    public class OrdersController : Controller, IGenericController<OrderDto>
    {

        private readonly IGenericHttpService<OrderDto> _genericHttpService;
        private readonly IAuthenticationService _authenticationService;

        public OrdersController(IGenericHttpService<OrderDto> genericHttpService, IAuthenticationService authenticationService)
        {           
            _genericHttpService = genericHttpService;
            _genericHttpService.controlador = "Orders";
            _genericHttpService.cliente = "TEST";

            _authenticationService = authenticationService;
        }

        public async Task LogOut()
        {

           
            await _authenticationService.SignOutAsync(HttpContext, CookieAuthenticationDefaults.AuthenticationScheme, new AuthenticationProperties() { });
            await _authenticationService.SignOutAsync(HttpContext, OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties() { RedirectUri = "home/prueba" });

            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme); //, new AuthenticationProperties{);
            //return new SignOutResult(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = Url.Action(nameof(Login)) });
        }

        public async Task<ActionResult<gridDto<OrderDto>>> Get([DataSourceRequest]DataSourceRequest request)
        {

            //return StatusCode(401);

            var query = Request.QueryString;
            var (sc, gridDto) = await _genericHttpService.HttpGetAsync(query);

            if (sc == HttpStatusCode.Redirect){ HttpContext.Response.ContentType = "text/html"; return StatusCode((int)sc); }; // Notification Redirect(HttpContext.Response.Headers["Location"]); //.FirstOrDefault(x => x.Key == "location"); //  StatusCode((int)sc); // Unauthorized();

            //return RedirectToAction(nameof(LogOut)); // new RedirectResult()
            //return StatusCode(401);
           // if (sc == HttpStatusCode.Redirect) return RedirectToAction(nameof(HomeController.LogOut), "home"); // return  new SignOutResult(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = Url.Action(nameof(HomeController.Login), "Home") }); // StatusCode((int)sc);

            return  StatusCode((int)sc, gridDto);            

        }

        public async Task<ActionResult> Post(OrderDto entityDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(error => error.ErrorMessage));            

            var (sc, entidad) = await _genericHttpService.HttPostAsync(entityDto);
            
            return StatusCode((int)sc, new[] { entidad }.ToDataSourceResult(new DataSourceRequest(), ModelState));                      

        }

        public async Task<ActionResult> Put(OrderDto entityDto, string id)
        {

            if (!ModelState.IsValid || entityDto.OrderId != Int32.Parse(id))            
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(error => error.ErrorMessage));

            var (sc, entidad) = await _genericHttpService.HttpPutAsync(entityDto, id.ToString());

            return StatusCode((int)sc, new[] { entidad }.ToDataSourceResult(new DataSourceRequest(), ModelState));          

        }

        public async Task<ActionResult> Delete(OrderDto entityDto, string id)
        {
            if (!ModelState.IsValid || entityDto.OrderId != Int32.Parse(id))            
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(error => error.ErrorMessage));

            var result = await _genericHttpService.HttpDeleteAsync(id.ToString());

            return StatusCode((int)result, new object[] { entityDto }.ToDataSourceResult(new DataSourceRequest(), ModelState));
           
        }

        //public async Task<ActionResult<gridDto<OrderDto>>> Get([DataSourceRequest]DataSourceRequest request)
        //{
        //    var query = Request.QueryString;
        //    var result = await _genericHttpService.HttpGetAsync(query);

        //    return Json(result);


        //    //var a = Request.QueryString;

        //    //var client = _httpClientFactory.CreateClient("TEST");
        //    //var response = await client.GetAsync($"Orders{a}").Result.Content.ReadAsStringAsync();            


        //    //var c = JsonConvert.DeserializeObject<gridDto<OrderDto>>(response, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });       

        //    //return Json(c);         

        //}


        //public async Task<ActionResult> Post([DataSourceRequest]DataSourceRequest request, OrderDto orderDto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(error => error.ErrorMessage));


        //    var result = await _genericHttpService.HttPostAsync(orderDto);


        //    return StatusCode((int)result.Key, (result.Value != null) ? new[] { result.Value }.ToDataSourceResult(request, ModelState) : null);

        //    //if (result.value != null)
        //    //{
        //    //    return Json(new[] { result.Item1 }.ToDataSourceResult(request, ModelState));
        //    //}
        //    //else {
        //    //    return StatusCode((int)result.Item2);
        //    //}


        //    //var client = _httpClientFactory.CreateClient("TEST");
        //    //using (var content = new StringContent(JsonConvert.SerializeObject(orderDto), System.Text.Encoding.UTF8, "application/json"))
        //    //{
        //    //    HttpResponseMessage result = await client.PostAsync("Orders", content);
        //    //    if (result.StatusCode == System.Net.HttpStatusCode.Created)
        //    //    {
        //    //        var c = JsonConvert.DeserializeObject<OrderDto>(result.Content.ReadAsStringAsync().Result, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });


        //    //        return Json(new[] { c }.ToDataSourceResult(request, ModelState));

        //    //        //return Ok();
        //    //    }
        //    //    string returnvalue = result.Content.ReadAsStringAsync().Result;
        //    //    return StatusCode((int)result.StatusCode);
        //    //    //throw new Exception($"Failed to PUT data : ({result.StatusCode}): {returnvalue}");
        //    //}

        //    //return Json(new[] { product }.ToDataSourceResult(request, ModelState));

        //}

        //public async Task<ActionResult> Put([DataSourceRequest] DataSourceRequest request, OrderDto orderdto, int id)
        //{

        //    //var id = Request.RouteValues.FirstOrDefault(x => x.Key == "id").Value;

        //    if (!ModelState.IsValid || orderdto.OrderId != id)
        //    {
        //        return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(error => error.ErrorMessage));
        //    }


        //    var client = _httpClientFactory.CreateClient("TEST");
        //        using (var content = new StringContent(JsonConvert.SerializeObject(orderdto), System.Text.Encoding.UTF8, "application/json"))
        //        {
        //            HttpResponseMessage result = await client.PutAsync($"Orders/{id}", content);  //.Result; // _httpClient.PostAsync(url, content).Result;
        //            if (result.StatusCode == System.Net.HttpStatusCode.Created)
        //            {
        //                var c = JsonConvert.DeserializeObject<OrderDto>(result.Content.ReadAsStringAsync().Result, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });


        //                return Json(new[] { c }.ToDataSourceResult(request, ModelState));
        //                //return Ok(); 
        //            }

        //            string returnValue = result.Content.ReadAsStringAsync().Result;
        //            return StatusCode((int)result.StatusCode);

        //            //throw new Exception($"Failed to PUT data: ({result.StatusCode}): {returnValue}");
        //        }


        //}

        //public async Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, OrderDto orderDto, int id)
        //{
        //    if (!ModelState.IsValid || orderDto.OrderId != id)
        //    {               
        //            return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(error => error.ErrorMessage));
        //    }


        //        var client = _httpClientFactory.CreateClient("TEST");

        //        HttpResponseMessage result = await client.DeleteAsync($"Orders/{id}");
        //        if (result.StatusCode == System.Net.HttpStatusCode.OK)
        //            return Json(new[] { orderDto }.ToDataSourceResult(request, ModelState));

        //        //return Ok();
        //        string returnvalue = result.Content.ReadAsStringAsync().Result;
        //        return StatusCode((int)result.StatusCode);
        //    //return NotFound(); // 
        //        //throw new Exception($"Failed to PUT data : ({result.StatusCode}): {returnvalue}");





        //    //return new ObjectResult(new DataSourceResult { Data = new[] { product }, Total = 1 });
        //}

    }
}