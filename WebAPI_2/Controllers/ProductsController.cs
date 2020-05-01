﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos.Contexto;
using Modelos.Modelos;
using Services;
using Services.Dtos;

namespace WebAPI_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Products, ProductDto> _genericRepository;
        private readonly IGenericRepository<Products, ProductCboDto> _genericRepositoryCbo;

        public ProductsController(IGenericRepository<Products, ProductDto> genericRepository, IGenericRepository<Products, ProductCboDto> genericRepositoryCbo)
        {
            _genericRepository = genericRepository;
            _genericRepositoryCbo = genericRepositoryCbo;
        }

        [HttpGet("Cbo")]
        public async Task<ActionResult<IEnumerable<ProductCboDto>>> GetProductsCbo()
        {

            var dest = await _genericRepositoryCbo.GetAllAsync();           

            return Ok(dest);
           
        }


        // GET: api/Customers // IGenericRepository IUnitOfWork
        [HttpGet]
        public async Task<ActionResult<gridDto<ProductDto>>> GetProducts([DataSourceRequest]DataSourceRequest request)
        {




            //DataSourceResult result = await _genericRepository.GetAsync(request);

            var dest = await _genericRepository.GetPropertiesAsync(request, p => p.Category); // "Category"); 

            //var resmedio = _mapper.Map <CustomersDto>(result.Data);

            //var dest = _mapper.Map<gridDto<CustomersDto>>(result); //   (result, typeof(DataSourceResult), typeof(CustomersDto<Customers>)); 
            //var dest = mapper.Map<Source<int>, Destination<int>>(source);

            //var mapeo = _mapper.Map<CustomersDto<Customers>>(result);


            //var result = await _context.Customers.ToDataSourceResultAsync(request);

            //var customersdto = new CustomersDto<Customers> { Data = (result.Data as List<Customers>), Total = result.Total, AggregateResults = result.AggregateResults, Errors = result.Errors };

            return Ok(dest);

            //return Ok(customersdto);


        }

        //// GET: api/Products
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Products>>> GetProducts()
        //{
        //    return await _context.Products.ToListAsync();
        //}

        //// GET: api/Products/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Products>> GetProducts(int id)
        //{
        //    var products = await _context.Products.FindAsync(id);

        //    if (products == null)
        //    {
        //        return NotFound();
        //    }

        //    return products;
        //}

        //// PUT: api/Products/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //// more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutProducts(int id, Products products)
        //{
        //    if (id != products.ProductId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(products).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProductsExists(id))
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

        //// POST: api/Products
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //// more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPost]
        //public async Task<ActionResult<Products>> PostProducts(Products products)
        //{
        //    _context.Products.Add(products);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetProducts", new { id = products.ProductId }, products);
        //}

        //// DELETE: api/Products/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Products>> DeleteProducts(int id)
        //{
        //    var products = await _context.Products.FindAsync(id);
        //    if (products == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Products.Remove(products);
        //    await _context.SaveChangesAsync();

        //    return products;
        //}

        //private bool ProductsExists(int id)
        //{
        //    return _context.Products.Any(e => e.ProductId == id);
        //}
    }
}