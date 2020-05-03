using System;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Modelos.Modelos;
using Services;
using Services.Dtos;

namespace WebAPI_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IGenericRepository<Orders, OrderDto> _genericRepository;

        public OrdersController(IGenericRepository<Orders, OrderDto> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        [HttpGet]
        public async Task<ActionResult<gridDto<OrderDto>>> GetOrders([DataSourceRequest]DataSourceRequest request)
        {

            var dest = await _genericRepository.GetPropertiesAsync(request, p => p.Customer, p => p.Employee);

            return Ok(dest);

        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> PostOrders(OrderDto orderDto)
        {

            var entidad = await _genericRepository.CreateAsync(orderDto);

            return StatusCode(201, entidad);

            //return StatusCode(201);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDto>> PutOrders(OrderDto orderdto, int id)
        {

            var entidad = await _genericRepository.UpdateAsync(x => x.OrderId == orderdto.OrderId, orderdto);

            return StatusCode(201, entidad);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderDto>> DeleteOrders(int id)
        {

            await _genericRepository.DeleteAsync(x => x.OrderId ==  id);

            return StatusCode(200);
        }
    }
}