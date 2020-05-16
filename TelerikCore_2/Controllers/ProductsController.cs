using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Dtos;
using Services.HttpServices;

namespace TelerikCore_2.Controllers
{
    public class ProductsController : Controller, IGenericController<ProductDto>
    {
      
        private readonly IGenericHttpService<ProductDto> _genericHttpService;

        public ProductsController(IGenericHttpService<ProductDto> genericHttpService)
        {         
            _genericHttpService = genericHttpService;
            _genericHttpService.controlador = "Products";
            _genericHttpService.cliente = "TEST";
        }     


        public async Task<ActionResult<gridDto<ProductDto>>> Get([DataSourceRequest] DataSourceRequest request)
        {
            var query = Request.QueryString;
            var result = await _genericHttpService.HttpGetAsync(query);

            return Json(result);
        }

        public Task<ActionResult> Post( ProductDto entityDto)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> Put( ProductDto entityDto, string id)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> Delete(ProductDto entityDto, string id)
        {
            throw new NotImplementedException();
        }





        //public async Task<ActionResult<gridDto<ProductDto>>> Get([DataSourceRequest]DataSourceRequest request)
        //{

        //    var a = Request.QueryString;

        //    var client = _httpClientFactory.CreateClient("TEST");
        //    var response = await client.GetAsync($"Products{a}").Result.Content.ReadAsStringAsync();           


        //    var c = JsonConvert.DeserializeObject<gridDto<ProductDto>>(response, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

        //    //var c = JsonConvert.DeserializeObject<DataSourceResult>(response, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });


        //    return Json(c);


        //    //var b = JsonConvert.DeserializeObject<List<Customers>>(response).AsQueryable();


        //}





        // GET: Products
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: Products/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Products/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Products/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Products/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Products/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Products/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Products/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}