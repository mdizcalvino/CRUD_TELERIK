using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modelos.Modelos;
using Services;
using Services.Dtos;

namespace WebAPI_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IGenericRepository<Employees, EmployeCboDto> _genericRepositoryCbo;
        private readonly IGenericRepository<Employees, EmployeeDto> _genericRepository;

        public EmployeesController(IGenericRepository<Employees, EmployeCboDto>  genericRepositoryCbo, IGenericRepository<Employees, EmployeeDto> genericRepository  )
        {
            _genericRepositoryCbo = genericRepositoryCbo;
            _genericRepository = genericRepository;
        }



        [HttpGet("Cbo")]
        public async Task<ActionResult<IEnumerable<EmployeCboDto>>> GetCategories()
        {

            var dest = await _genericRepositoryCbo.GetAllAsync(); //.GetAsync(request);

            //var resmedio = _mapper.Map <CustomersDto>(result.Data);

            return Ok(dest);

            //return await _context.Categories.ToListAsync();
        }


    }
}