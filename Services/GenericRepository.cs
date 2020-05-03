using AutoMapper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.EntityFrameworkCore;
using Modelos.Modelos;
using Services.Dtos;
using Services.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IGenericRepository<TIN, TOUT> 
        where TIN : class
        where TOUT : class
    {    

        Task<gridDto<TOUT>> GetAsync(DataSourceRequest dataSourceRequest);
        Task<gridDto<TOUT>> GetPropertiesAsync(DataSourceRequest dataSourceRequest, params Expression<Func<TIN, object>>[] includeProperties);
        Task<gridDto<TOUT>> GetPropertiesByIdAsync(DataSourceRequest dataSourceRequest, Expression<Func<TIN, bool>> whereCondition = null, params Expression<Func<TIN, object>>[] includeProperties);

        Task<IList<TOUT>> GetAllAsync();      

        //Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> whereCondition = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");

        Task<TOUT> CreateAsync(TOUT entity_out);
        Task<TOUT> UpdateAsync(Expression<Func<TIN, bool>> predicate,TOUT entityDto);

        Task DeleteAsync(Expression<Func<TIN, bool>> predicate);
        
    }

    public class GenericRepository<TIN, TOUT> : IGenericRepository<TIN, TOUT>
        where TIN : class
        where TOUT : class
    {

        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public GenericRepository(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper; 
        }

        //public virtual IEnumerable<TDto> MapToDtoList<TEntity, TDto>(IEnumerable<TEntity> entity)
        //where TEntity : class
        //where TDto : class
        //{

        //    Mapper mapper;

        //    mapper.cre.CreateMap<TEntity, TDto>();
        //    return Mapper.Map<IEnumerable<TEntity>, IEnumerable<TDto>>(entity);
        //}

        //public virtual IEnumerable<TEntity> MapToEntityList<TDto, TEntity>(IEnumerable<TDto> dto)
        //    where TDto : class
        //    where TEntity : class
        //{
        //    Mapper.CreateMap<TDto, TEntity>();
        //    return Mapper.Map<IEnumerable<TDto>, IEnumerable<TEntity>>(dto);
        //}




        public async Task<TOUT> CreateAsync(TOUT entity_out)
        {
            try
            {
                var entidad = _mapper.Map<TIN>(entity_out);
                await _unitofWork._context.Set<TIN>().AddAsync(entidad);
                _unitofWork.Commit();
                return _mapper.Map<TOUT>(entidad);
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }


        }

        public async Task<TOUT> UpdateAsync(Expression<Func<TIN, bool>> predicate, TOUT entityDto)
        {
            try
            {
                var c = await _unitofWork._context.Set<TIN>().FirstAsync(predicate);

                var entidad = _mapper.Map(entityDto, c, typeof(TOUT), typeof(TIN));
                _unitofWork._context.Set<TIN>().Attach((TIN)entidad);
                _unitofWork._context.Entry(entidad).State = EntityState.Modified;
                _unitofWork.Commit();
                return _mapper.Map<TOUT>(entidad);
            }
            catch(Exception)
            {
                throw new NotImplementedException();
            }
        }

        public async Task DeleteAsync(Expression<Func<TIN, bool>> predicate)
        {
            try
            {
                var entidad = await _unitofWork._context.Set<TIN>().FirstAsync(predicate);

                //if (customers == null)
                //{
                //    return NotFound();
                //}

                _unitofWork._context.Set<TIN>().Remove(entidad);
                _unitofWork.Commit();
            }
            catch(Exception)
            {
                throw new NotImplementedException();
            }
        }



        //     if (product.Category != null)
        //                {
        //                    entity.CategoryID = product.Category.CategoryID;
        //                }

        //db.Products.Attach(entity);
        //                db.Entry(entity).State = EntityState.Modified;
        //                db.SaveChanges();



        public async Task<IList<TOUT>> GetAllAsync()
        {
            var result = await _unitofWork._context.Set<TIN>().AsNoTracking().ToListAsync();
            var dest = _mapper.Map<IList<TOUT>>(result);
            return dest;
        }

        public async Task<gridDto<TOUT>> GetPropertiesByIdAsync(DataSourceRequest dataSourceRequest, Expression<Func<TIN, bool>> whereCondition = null, params Expression<Func<TIN, object>>[] includeProperties)
        {           

            IQueryable<TIN> query = _unitofWork._context.Set<TIN>().AsNoTracking();

            if (whereCondition != null) { query = query.Where(whereCondition); }

            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            DataSourceResult result = (await query.ToDataSourceResultAsync(dataSourceRequest) as DataSourceResult);

            var dest = _mapper.Map<gridDto<TOUT>>(result);
            return dest;

        }


        public async Task<gridDto<TOUT>> GetPropertiesAsync(DataSourceRequest dataSourceRequest, params Expression<Func<TIN, object>>[] includeProperties )
        {

            IQueryable<TIN> query = _unitofWork._context.Set<TIN>().AsNoTracking();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            DataSourceResult result = (await query.ToDataSourceResultAsync(dataSourceRequest) as DataSourceResult);
            
            var dest = _mapper.Map<gridDto<TOUT>>(result);
            return dest;
        }

        public async Task<gridDto<TOUT>> GetAsync(DataSourceRequest dataSourceRequest)
        {                   
      
            DataSourceResult result = (await _unitofWork._context.Set<TIN>().AsNoTracking().ToDataSourceResultAsync(dataSourceRequest) as DataSourceResult);

            var dest = _mapper.Map<gridDto<TOUT>>(result);
            return dest; 
            
        }

        //public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> whereCondition = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        //{
        //    IQueryable<T> query = _unitofWork._context.Set<T>();

        //    if(whereCondition != null) { query = query.Where(whereCondition); }

        //    foreach( var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        //    {
        //        query = query.Include(includeProperty);
        //    }

        //    if(orderBy != null)
        //    { 
        //        return await orderBy(query).ToListAsync();
        //    }
        //    else
        //    {
        //        return await query.ToListAsync();
        //    }
        //}




    }
}
