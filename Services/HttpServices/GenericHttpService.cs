using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Dtos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Services.HttpServices
{
   


    public interface IGenericHttpService<T> where T : class
    {
        public string controlador { set; }      


        Task<gridDto<T>> HttpGetAsync(QueryString query);
        //Task<Tuple<T, HttpStatusCode>> HttPostAsync(T entidadDto);
        //Task<ActionResult> HttPostAsync(T entidadDto);
        Task<KeyValuePair<HttpStatusCode, T>> HttPostAsync(T entidadDto);

        public struct CustomResponse
        {
            private readonly T entidad;
            private readonly int statusCode;

            public CustomResponse(T entidad, int statusCode)
            {


                this.entidad = entidad;
                this.statusCode = statusCode;
            }

            //// none of the following can be negative!!
            //public int Start => start;
            //public int End => end;
            //public int Length => end - start;
        }





    }
    public class GenericHttpService<T> :  IGenericHttpService<T> where T : class
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GenericHttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
       

        public string controlador { get; set; }


        public async Task<gridDto<T>> HttpGetAsync(QueryString query)
        {
            

            var client = _httpClientFactory.CreateClient("TEST");
            var response = await client.GetAsync($"{controlador}{query}").Result.Content.ReadAsStringAsync();

            var entidad = JsonConvert.DeserializeObject<gridDto<T>>(response, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

            return entidad as gridDto<T>;
        }

        //public async Task<Tuple<T, HttpStatusCode>> HttPostAsync(T entidadDto)
        //{
        //    var client = _httpClientFactory.CreateClient("TEST");
        //    using (var content = new StringContent(JsonConvert.SerializeObject(entidadDto), System.Text.Encoding.UTF8, "application/json"))
        //    {
        //        HttpResponseMessage result = await client.PostAsync(controlador, content);
        //        if (result.StatusCode == System.Net.HttpStatusCode.Created)
        //        {
        //            var c = JsonConvert.DeserializeObject<T>(result.Content.ReadAsStringAsync().Result, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

        //            return Tuple.Create<T, HttpStatusCode>(c,result.StatusCode);
        //            //return Json(new[] { c }.ToDataSourceResult(request, ModelState));

        //            //return Ok();
        //        }
        //        string returnvalue = result.Content.ReadAsStringAsync().Result;
        //        return Tuple.Create<T, HttpStatusCode>(null , result.StatusCode);
        //        //throw new Exception($"Failed to PUT data : ({result.StatusCode}): {returnvalue}");
        //    }
        //}

        public async Task<KeyValuePair<HttpStatusCode, T>> HttPostAsync(T entidadDto)
        {
            var client = _httpClientFactory.CreateClient("TEST");
            using (var content = new StringContent(JsonConvert.SerializeObject(entidadDto), System.Text.Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result = await client.PostAsync(controlador, content);
                if (result.StatusCode == System.Net.HttpStatusCode.Created)
                {
                  
                    var entidad = JsonConvert.DeserializeObject<T>(result.Content.ReadAsStringAsync().Result, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

                    return   new KeyValuePair<HttpStatusCode, T>(result.StatusCode, entidad);                    
                    
                    //var d = ValueTuple.Create<T, int>(c, (int)result.StatusCode);              

                }
                string returnvalue = result.Content.ReadAsStringAsync().Result;

                return new KeyValuePair<HttpStatusCode, T>(result.StatusCode, null);

                
            }
        }
    }
}
