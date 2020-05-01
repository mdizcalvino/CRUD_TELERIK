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

        public OrdersController(IGenericRepository<Orders, OrderDto>  genericRepository)
        {
            _genericRepository = genericRepository; 
        }

        [HttpGet]
        public async Task<ActionResult<gridDto<OrderDto>>> GetOrders([DataSourceRequest]DataSourceRequest request)
        {          

            var dest = await _genericRepository.GetPropertiesAsync(request, p => p.Customer, p => p.Employee);           

            return Ok(dest);       

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutCustomers([DataSourceRequest] DataSourceRequest request, OrderDto orderdto)
        {

            //DateTime birthdate;
            //var isDateValid = DateTime.ParseExact(.TryParse(orderdto.RequiredDate, out birthday);
            //if (!isDateValid) ModelState.AddModelError("Birthdate", "Birthday needs to be a valid date.");


            await _genericRepository.UpdateAsync(x => x.OrderId == orderdto.OrderId, orderdto); //.GetAsync(request);

            return StatusCode(201);
          
        }
    }
}