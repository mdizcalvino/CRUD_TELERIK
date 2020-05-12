using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modelos.Modelos;
using Services;
using Services.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI_2.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireAdministratorRole")]
    [ApiController]
    public class CombosController : ControllerBase
    {

                              
        [HttpGet("OrdersCbos")]
        public async Task<ActionResult<List<KeyValuePair<string, object>>>> GetOrdersCbos()
        {

            List<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("customers", await HttpContext.RequestServices.GetService<IGenericRepository<Customers, CustomersCboDto>>().GetAllAsync()),
                new KeyValuePair<string, object>("employees", await HttpContext.RequestServices.GetService<IGenericRepository<Employees, EmployeCboDto>>().GetAllAsync()),
                new KeyValuePair<string, object>("products", await HttpContext.RequestServices.GetService<IGenericRepository<Products, ProductDto>>().GetAllAsync())
            };

            return Ok(result);

        }

    }
}