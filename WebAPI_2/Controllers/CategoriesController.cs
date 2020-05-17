using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class CategoriesController : ControllerBase
    {
        private readonly IGenericRepository<Categories, CategoryDto> _genericRepository;
        private readonly IUnitOfWork _unitofWork;

        //private readonly ApplicationDBContext _context;

        public CategoriesController(IGenericRepository<Categories, CategoryDto> genericRepository, IUnitOfWork unitofWork )
        {
            _genericRepository = genericRepository;
            _unitofWork = unitofWork;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categories>>> GetCategories()
        {

            var dest = await _genericRepository.GetAllAsync(); //.GetAsync(request);

            //var resmedio = _mapper.Map <CustomersDto>(result.Data);

            

            return Ok(dest);

            //return await _context.Categories.ToListAsync();
        }

        // GET: api/Categories/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Categories>> GetCategories(int id)
        //{
        //    //var categories = await _context.Categories.FindAsync(id);

        //    //if (categories == null)
        //    //{
        //    //    return NotFound();
        //    //}

        //    //return categories;
        //}

        // PUT: api/Categories/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategories(int id, Categories categories)
        {
            //if (id != categories.CategoryId)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(categories).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!CategoriesExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return NoContent();
        }

        // POST: api/Categories
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPost]
        //public async Task<ActionResult<Categories>> PostCategories(Categories categories)
        //{
        //    _context.Categories.Add(categories);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetCategories", new { id = categories.CategoryId }, categories);
        //}

        //// DELETE: api/Categories/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Categories>> DeleteCategories(int id)
        //{
        //    var categories = await _context.Categories.FindAsync(id);
        //    if (categories == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Categories.Remove(categories);
        //    await _context.SaveChangesAsync();

        //    return categories;
        //}

        //private bool CategoriesExists(int id)
        //{
        //    return _context.Categories.Any(e => e.CategoryId == id);
        //}
    }
}
