﻿using Microsoft.AspNetCore.Http;
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
using Newtonsoft.Json.Linq;


using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using IdentityServer4.AccessTokenValidation;
using System.Globalization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using IdentityModel.Client;

namespace Services.HttpServices
{
   


    public interface IGenericHttpService<T>  where T : class
    {

        public string cliente { set; }
        public string controlador { set; }  
       
        
        Task<KeyValuePair<HttpStatusCode, List<KeyValuePair<string, object>>>> HttpCbosAsync();

        Task<(HttpStatusCode sc, gridDto<T> gridDto)> HttpGetAsync(QueryString query);
        Task<(HttpStatusCode sc, gridDto<T> gridDto)> HttpGetDetailAsync(QueryString query, string id);
        Task<(HttpStatusCode sc, T entidad)> HttPostAsync(T entidadDto);
        Task<(HttpStatusCode sc, T entidad)> HttpPutAsync(T entidadDto, string id);
        Task<HttpStatusCode> HttpDeleteAsync(string id);
        Task<HttpClient> GetHttpClientWithToken();
        Task<(string at, int error)> RefreshToKen();

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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TokenClient _tokenClient;

        public GenericHttpService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, TokenClient tokenClient)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _tokenClient = tokenClient;
        }
       

        public string controlador {private get; set; }
        public string cliente {private get; set; }


       

        public async Task<(string at, int error)> RefreshToKen()
        {
           
            var rt = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            var tokenResult = await _tokenClient.RequestRefreshTokenAsync(rt);

            if (!tokenResult.IsError)
            {
                //var expiresAt = (DateTime.UtcNow + TimeSpan.FromSeconds(tokenResult.ExpiresIn)).ToString("o", CultureInfo.InvariantCulture);
                var expiresAt = (DateTime.Now + TimeSpan.FromSeconds(tokenResult.ExpiresIn)).ToString("o", CultureInfo.InvariantCulture);

                var authService = _httpContextAccessor.HttpContext.RequestServices.GetService<IAuthenticationService>();
                AuthenticateResult authenticateResult = await authService.AuthenticateAsync(_httpContextAccessor.HttpContext, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = authenticateResult.Properties;

                properties.UpdateTokenValue(OpenIdConnectParameterNames.RefreshToken, tokenResult.RefreshToken);
                properties.UpdateTokenValue(OpenIdConnectParameterNames.AccessToken, tokenResult.AccessToken);
                //properties.UpdateTokenValue(OpenIdConnectParameterNames.ExpiresIn, expiresAt);
                properties.UpdateTokenValue("expires_at", expiresAt); //OpenIdConnectParameterNames..ExpiresIn
                
                //properties.AllowRefresh = true;
                //ShouldRenew = true;

                await authService.SignInAsync(_httpContextAccessor.HttpContext, CookieAuthenticationDefaults.AuthenticationScheme, authenticateResult.Principal, authenticateResult.Properties);             

                return (await _httpContextAccessor.HttpContext.GetTokenAsync("access_token"), 0);
            }

            return  (await _httpContextAccessor.HttpContext.GetTokenAsync("access_token"), 1);
        }

        public async Task<HttpClient> GetHttpClientWithToken()
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            int error;

            DateTime accessTokenExpiresAt = DateTime.Parse(await _httpContextAccessor.HttpContext.GetTokenAsync("expires_at"), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            if(DateTime.Now.CompareTo(accessTokenExpiresAt.AddMinutes(-1)) >= 0) { (accessToken, error) = await RefreshToKen(); }

            var client = _httpClientFactory.CreateClient($"{cliente}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(IdentityServerAuthenticationDefaults.AuthenticationScheme , accessToken);

            return client;
           
        }
        
        public async Task<(HttpStatusCode sc, gridDto<T> gridDto)> HttpGetAsync(QueryString query)
        {
          
            var client = await GetHttpClientWithToken();

            var result = await client.GetAsync($"{controlador}{query}"); //.Result.Content.ReadAsStringAsync();

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var entidad = JObject.Parse(result.Content.ReadAsStringAsync().Result).ToObject<gridDto<T>>();
                return (result.StatusCode, entidad);
            }

            return (result.StatusCode, null);
            //var entidad = JsonConvert.DeserializeObject<gridDto<T>>(response, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

            //return entidad as gridDto<T>;

            //var client = _httpClientFactory.CreateClient($"{cliente}");
            //var response = await client.GetAsync($"{controlador}{query}").Result.Content.ReadAsStringAsync();

            //var entidad = JsonConvert.DeserializeObject<gridDto<T>>(response, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

            //return entidad as gridDto<T>;
        }


        public async Task<(HttpStatusCode sc, gridDto<T> gridDto)> HttpGetDetailAsync(QueryString query, string id)
        {
           
            var client = await GetHttpClientWithToken();

            var result = await client.GetAsync($"{controlador}/{id}{query}"); 

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var entidad = JObject.Parse(result.Content.ReadAsStringAsync().Result).ToObject<gridDto<T>>();
                return (result.StatusCode, entidad);
            }

            return (result.StatusCode, null);
        }

        public async Task<KeyValuePair<HttpStatusCode, List<KeyValuePair<string, object>>>> HttpCbosAsync()
        {

            var client = await GetHttpClientWithToken();

            HttpResponseMessage result = await client.GetAsync($"Combos/{controlador}Cbos");
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var entidad = JsonConvert.DeserializeObject<List<KeyValuePair<string, object>>>(result.Content.ReadAsStringAsync().Result, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
               
                return new KeyValuePair<HttpStatusCode, List<KeyValuePair<string, object>>>(result.StatusCode, entidad);
            }
            
            return new KeyValuePair<HttpStatusCode, List<KeyValuePair<string,object>>>(result.StatusCode, null);
           
        }



        public async Task<(HttpStatusCode sc, T entidad)> HttPostAsync(T entidadDto)
        {
            //var client = _httpClientFactory.CreateClient($"{cliente}");
            var client = await GetHttpClientWithToken();
            using (var content = new StringContent(JsonConvert.SerializeObject(entidadDto), Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result = await client.PostAsync($"{controlador}", content); //controlador
                if (result.StatusCode == System.Net.HttpStatusCode.Created)
                {                  
                    var entidad = JsonConvert.DeserializeObject<T>(result.Content.ReadAsStringAsync().Result, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

                    return (result.StatusCode, entidad);                                
                    
                }
               
                return (result.StatusCode, entidadDto);                
                
            }
        }

        public async Task<(HttpStatusCode sc, T entidad)> HttpPutAsync(T entidadDto, string id)
        {
            
            var client = await GetHttpClientWithToken();
            using (var content = new StringContent(JsonConvert.SerializeObject(entidadDto), Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result = await client.PutAsync($"{controlador}/{id}", content); 
                if (result.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var entidad = JsonConvert.DeserializeObject<T>(result.Content.ReadAsStringAsync().Result, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
                   
                    return (result.StatusCode, entidad);
                    
                }
               
                return (result.StatusCode, entidadDto);

            }
        }

        public async Task<HttpStatusCode> HttpDeleteAsync(string id)
        {
            //var client = _httpClientFactory.CreateClient($"{cliente}");
            var client = await GetHttpClientWithToken();
            HttpResponseMessage result = await client.DeleteAsync($"{controlador}/{id}");

            return (result.StatusCode);
           
        }
       
    }
}
