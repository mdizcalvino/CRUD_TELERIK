using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modelos.Modelos;
using Services;
using Services.Dtos;

namespace WebAPI_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IGenericRepository<OrderDetails, OrderDetailsDto> _genericRepository;

        public OrderDetailsController(IGenericRepository<OrderDetails,OrderDetailsDto> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<gridDto<OrderDetailsDto>>> GetOrdersDetails([DataSourceRequest]DataSourceRequest request, string id)
        {
          
            var dest = await _genericRepository.GetPropertiesByIdAsync(request, p => p.OrderId == Int32.Parse(id) , p => p.Product);

            return Ok(dest);

        }
    }
}