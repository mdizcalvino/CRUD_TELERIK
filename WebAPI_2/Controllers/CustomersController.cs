using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos.Contexto;
using Modelos.Modelos;
using System.Text.Json;
using System.Collections;
using Kendo.Mvc.Infrastructure;
using Newtonsoft.Json;
using Services;
using AutoMapper;
using Services.Dtos;
using Services.Profiles;
using Microsoft.AspNetCore.Authorization;



//using Newtonsoft.Json.Serialization;



namespace WebAPI_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        //private readonly ApplicationDBContext _context;
        private readonly IGenericRepository<Customers, CustomersCboDto> _genericRepositoryCbo;
        private readonly IGenericRepository<Customers, CustomersDto> _genericRepository;

        //private readonly IUnitofWork _unitofWork;
        //private readonly IMapper _mapper;

        //public CustomersController(ApplicationDBContext context)
        //{
        //    _context = context;
        //}


        public CustomersController(IGenericRepository<Customers, CustomersCboDto> genericRepositoryCbo, IGenericRepository<Customers, CustomersDto> genericRepository) //, IUnitofWork unitofWork, IMapper mapper )
        {
            _genericRepositoryCbo = genericRepositoryCbo;
            _genericRepository = genericRepository;
            //_unitofWork = unitofWork;
            //_mapper = mapper;
        }


        //// GET: api/Customers  // funcionando como string
        //[HttpGet]
        //public async Task<ActionResult<string>> GetCustomers([DataSourceRequest]DataSourceRequest request)
        //{
        //    //var result = _context.Customers; //.Page; // (request.Page, request.PageSize);

        //    //var todata = result.ToDataSourceResult(request);


        //    var result = await _context.Customers.ToDataSourceResultAsync(request);


        //    var ver = JsonConvert.SerializeObject(result, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

        //    return ver;    

        //    //return new JsonResult(result);
        //}


        //public class GridDtoBase
        //{          

        //    public int Total { get; set; }
        //    public IEnumerable<AggregateResult> AggregateResults { get; set; }
        //    public object Errors { get; set; }
        //}

        //public class CustomersDto<T> : GridDtoBase
        //{
        //    public IList<T> Data { get; set; }   

        //}


        [HttpGet("Cbo")]
        public async Task<ActionResult<IEnumerable<CustomersCboDto>>> GetCustomersCbo()
        {

            var dest = await _genericRepositoryCbo.GetAllAsync(); //.GetAsync(request);

            //var resmedio = _mapper.Map <CustomersDto>(result.Data);

            return Ok(dest);

            //return await _context.Categories.ToListAsync();
        }

    
        // GET: api/Customers // IGenericRepository IUnitOfWork
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<gridDto<CustomersDto>>> GetCustomers([DataSourceRequest]DataSourceRequest request)
        {


            

            //DataSourceResult result = await _genericRepository.GetAsync(request);

            var dest = await  _genericRepository.GetAsync(request);

            //var resmedio = _mapper.Map <CustomersDto>(result.Data);

            //var dest = _mapper.Map<gridDto<CustomersDto>>(result); //   (result, typeof(DataSourceResult), typeof(CustomersDto<Customers>)); 
            //var dest = mapper.Map<Source<int>, Destination<int>>(source);

            //var mapeo = _mapper.Map<CustomersDto<Customers>>(result);


            //var result = await _context.Customers.ToDataSourceResultAsync(request);

            //var customersdto = new CustomersDto<Customers> { Data = (result.Data as List<Customers>), Total = result.Total, AggregateResults = result.AggregateResults, Errors = result.Errors };

            return Ok(dest);

            //return Ok(customersdto);

        
        }




        // GET: api/Customers  #FUNCIONANDO OK#
        //[HttpGet]
        //public async Task<ActionResult<CustomersDto<Customers>>> GetCustomers([DataSourceRequest]DataSourceRequest request, string fake = "")
        //{
          
        //    var result = await _context.Customers.ToDataSourceResultAsync(request);

        //    var customers = new CustomersDto<Customers> { Data = (result.Data as List<Customers>), Total = result.Total, AggregateResults = result.AggregateResults, Errors = result.Errors };

           
        //    return Ok(customers); 

        //    //var ver = JsonConvert.SerializeObject(result, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

        //    //return new JsonResult(result);
        //}



        // GET: api/Customers/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Customers>> GetCustomers(string id)
        //{
        //    //var customers = await _context.Customers.FindAsync(id);

        //    //if (customers == null)
        //    //{
        //    //    return NotFound();
        //    //}

        //    //return customers;
        //}

       
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCustomers([DataSourceRequest] DataSourceRequest request, CustomersDto customerdto)
        {
            
            //if (id != customerdto.CustomerId)
            //{
            //    return BadRequest();
            //}


            await _genericRepository.UpdateAsync(x => x.CustomerId ==  customerdto.CustomerId, customerdto); //.GetAsync(request);

            return StatusCode(201);
            //_context.Entry(customers).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!CustomersExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}
            // ControllerBase. Ok(); // new StatusCodeResult(200);
            //return NoContent();
        }


        // PUT: api/Customers/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCustomers(string id, Customers customers)
        //{
        //    if (id != customers.CustomerId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(customers).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CustomersExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        [HttpPost]
        public async Task<ActionResult<Customers>> PostCustomers(CustomersDto customersDto)
        {

            await _genericRepository.CreateAsync(customersDto); //.UpdateAsync(x => x.CustomerId == customerdto.CustomerId, customerdto); //.GetAsync(request);

            return StatusCode(201);


            //_context.Customers.Add(customers);
            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateException)
            //{
            //    if (CustomersExists(customers.CustomerId))
            //    {
            //        return Conflict();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return CreatedAtAction("GetCustomers", new { id = customers.CustomerId }, customers);
        }



        // POST: api/Customers
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPost]
        //public async Task<ActionResult<Customers>> PostCustomers(Customers customers)
        //{
        //    _context.Customers.Add(customers);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (CustomersExists(customers.CustomerId))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetCustomers", new { id = customers.CustomerId }, customers);
        //}

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customers>> DeleteCustomers(string id)
        {

            await _genericRepository.DeleteAsync(x => x.CustomerId == id);

            return StatusCode(200);

            //var customers = await _context.Customers.FindAsync(id);
            //if (customers == null)
            //{
            //    return NotFound();
            //}

            //_context.Customers.Remove(customers);
            //await _context.SaveChangesAsync();

            //return customers;
        }

        //// DELETE: api/Customers/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Customers>> DeleteCustomers(string id)
        //{
        //    var customers = await _context.Customers.FindAsync(id);
        //    if (customers == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Customers.Remove(customers);
        //    await _context.SaveChangesAsync();

        //    return customers;
        //}

        //private bool CustomersExists(string id)
        //{
        //    //return _context.Customers.Any(e => e.CustomerId == id);
        //}
    }
}
