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

        public string cliente { set; }
        public string controlador { set; }  
       
        
        Task<KeyValuePair<HttpStatusCode, List<KeyValuePair<string, object>>>> HttpCbosAsync();

        Task<gridDto<T>> HttpGetAsync(QueryString query);        
        Task<KeyValuePair<HttpStatusCode, T>> HttPostAsync(T entidadDto);
        Task<KeyValuePair<HttpStatusCode, T>> HttpPutAsync(T entidadDto, string id);
        Task<KeyValuePair<HttpStatusCode, T>> HttpDeleteAsync(T entidadDto, string id);

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
       

        public string controlador {private get; set; }
        public string cliente {private get; set; }

        
        public async Task<gridDto<T>> HttpGetAsync(QueryString query)
        {            

            var client = _httpClientFactory.CreateClient($"{cliente}");
            var response = await client.GetAsync($"{controlador}{query}").Result.Content.ReadAsStringAsync();

            var entidad = JsonConvert.DeserializeObject<gridDto<T>>(response, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

            return entidad as gridDto<T>;
        }

        public async Task<KeyValuePair<HttpStatusCode, List<KeyValuePair<string, object>>>> HttpCbosAsync()
        {
            var client = _httpClientFactory.CreateClient($"{cliente}");
           
            HttpResponseMessage result = await client.GetAsync($"Combos/{controlador}Cbos");
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var entidad = JsonConvert.DeserializeObject<List<KeyValuePair<string, object>>>(result.Content.ReadAsStringAsync().Result, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

                return new KeyValuePair<HttpStatusCode, List<KeyValuePair<string, object>>>(result.StatusCode, entidad);

            }

            string returnValue = result.Content.ReadAsStringAsync().Result;
            return new KeyValuePair<HttpStatusCode, List<KeyValuePair<string,object>>>(result.StatusCode, null);

           
        }



        public async Task<KeyValuePair<HttpStatusCode, T>> HttPostAsync(T entidadDto)
        {
            var client = _httpClientFactory.CreateClient($"{cliente}");
            using (var content = new StringContent(JsonConvert.SerializeObject(entidadDto), System.Text.Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result = await client.PostAsync("123", content); //controlador
                if (result.StatusCode == System.Net.HttpStatusCode.Created)
                {                  
                    var entidad = JsonConvert.DeserializeObject<T>(result.Content.ReadAsStringAsync().Result, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

                    return   new KeyValuePair<HttpStatusCode, T>(result.StatusCode, entidad);                 
                    
                }
                string returnvalue = result.Content.ReadAsStringAsync().Result;

                return new KeyValuePair<HttpStatusCode, T>(result.StatusCode, null);
                
            }
        }

        public async Task<KeyValuePair<HttpStatusCode, T>> HttpPutAsync(T entidadDto, string id)
        {
            var client = _httpClientFactory.CreateClient($"{cliente}");
            using (var content = new StringContent(JsonConvert.SerializeObject(entidadDto), System.Text.Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result = await client.PutAsync($"{controlador}/{id}", content); 
                if (result.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var entidad = JsonConvert.DeserializeObject<T>(result.Content.ReadAsStringAsync().Result, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

                    return new KeyValuePair<HttpStatusCode, T>(result.StatusCode, entidad);
                    
                }

                string returnValue = result.Content.ReadAsStringAsync().Result;
                return new KeyValuePair<HttpStatusCode, T>(result.StatusCode, null);
               
            }
        }

        public async Task<KeyValuePair<HttpStatusCode, T>> HttpDeleteAsync(T entidadDto, string id)
        {
            var client = _httpClientFactory.CreateClient($"{cliente}");

            HttpResponseMessage result = await client.DeleteAsync($"{controlador}/{id}");
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
                return new KeyValuePair<HttpStatusCode, T>(result.StatusCode, entidadDto);

            string returnvalue = result.Content.ReadAsStringAsync().Result;
            return new KeyValuePair<HttpStatusCode, T>(result.StatusCode, null);
        }

       
    }
}
