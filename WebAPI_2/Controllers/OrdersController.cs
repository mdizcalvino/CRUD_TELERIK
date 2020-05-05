using System;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Modelos.Modelos;
using Services;
using Services.Dtos;
using Services.HttpServices;

namespace WebAPI_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase, IGenericApiController<OrderDto>
    {
        private readonly IGenericRepository<Orders, OrderDto> _genericRepository;

        public OrdersController(IGenericRepository<Orders, OrderDto> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        [HttpGet]
        public async Task<ActionResult<gridDto<OrderDto>>> GetApi([DataSourceRequest]DataSourceRequest request)
        {

            var dest = await _genericRepository.GetPropertiesAsync(request, p => p.Customer, p => p.Employee);

            return Ok(dest);

        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> PostApi(OrderDto entityDto)
        {

            var entidad = await _genericRepository.CreateAsync(entityDto);

            return StatusCode(201, entidad);

            //return StatusCode(201);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDto>> PutApi(OrderDto entityDto, string id)
        {

            var entidad = await _genericRepository.UpdateAsync(x => x.OrderId == entityDto.OrderId, entityDto);

            return StatusCode(201, entidad);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderDto>> DeleteApi(string id)
        {

            await _genericRepository.DeleteAsync(x => x.OrderId ==  Int32.Parse(id));

            return StatusCode(200);
        }
    }
}