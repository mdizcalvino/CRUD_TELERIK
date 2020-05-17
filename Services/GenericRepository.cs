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
        //Task<IEnumerable<T>> GetAsync(DataSourceRequest dataSourceRequest);

        //Task<DataSourceResult> GetAsync(DataSourceRequest dataSourceRequest);
        Task<gridDto<TOUT>> GetAsync(DataSourceRequest dataSourceRequest);
        Task<gridDto<TOUT>> GetPropertiesAsync(DataSourceRequest dataSourceRequest, params Expression<Func<TIN, object>>[] includeProperties);
        Task<gridDto<TOUT>> GetPropertiesByIdAsync(DataSourceRequest dataSourceRequest, Expression<Func<TIN, bool>> whereCondition = null, params Expression<Func<TIN, object>>[] includeProperties);

        Task<IList<TOUT>> GetAllAsync();

        //Task<>

        //Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> whereCondition = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");

        Task CreateAsync(TOUT entity_out);
        Task UpdateAsync(Func<TIN, bool> predicate,TOUT entityDto);

        Task DeleteAsync(string id);
        
    }

    public class GenericRepository<TIN, TOUT> : IGenericRepository<TIN, TOUT>
        where TIN : class
        where TOUT : class
    {

        private readonly IUnitOfWork _unitofWork;
        private readonly IMapper _mapper;

        public GenericRepository(IUnitOfWork unitofWork, IMapper mapper)
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




        public async Task CreateAsync(TOUT entity_out)
        {
            try
            {
                var entidad = _mapper.Map<TIN>(entity_out);
                await _unitofWork._context.Set<TIN>().AddAsync(entidad);
                _unitofWork.Commit();
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }


        }

        public async Task UpdateAsync(Func<TIN, bool> predicate, TOUT entityDto)
        {

            var c = _unitofWork._context.Set<TIN>().FirstOrDefault(predicate);

            var entidad = _mapper.Map(entityDto, c, typeof(TOUT), typeof(TIN));  //  <TIN>(entityDto);
            _unitofWork._context.Set<TIN>().Attach((TIN)entidad); //.Property(    ..Collection(t => t..Aggregate     .Aggregate( .Property()..Navigations;
            _unitofWork._context.Entry(entidad).State = EntityState.Modified;
            _unitofWork.Commit();
        }

        public async Task DeleteAsync(string id)
        {
            var entidad = await _unitofWork._context.Set<TIN>().FindAsync(id);
            
            //if (customers == null)
            //{
            //    return NotFound();
            //}

            _unitofWork._context.Set<TIN>().Remove(entidad);
            _unitofWork.Commit(); // await _context.SaveChangesAsync();

            
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


            //return includeProperties
            //    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));


            var dest = _mapper.Map<gridDto<TOUT>>(result);
            return dest;
        }


        public async Task<gridDto<TOUT>> GetPropertiesAsync(DataSourceRequest dataSourceRequest, params Expression<Func<TIN, object>>[] includeProperties )
        {

            //var ver = _unitofWork._context.Products.Include("Category"); //  .Set<TIN>();
            ////var query = await ver.Include<TIN>(includeProperties).ToListAsync();



            //DataSourceResult result = (await _unitofWork._context.Set<TIN>().Include(includeProperties).ToDataSourceResultAsync(dataSourceRequest) as DataSourceResult);

            IQueryable<TIN> query = _unitofWork._context.Set<TIN>().AsNoTracking();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            DataSourceResult result = (await query.ToDataSourceResultAsync(dataSourceRequest) as DataSourceResult);

            
            //return includeProperties
            //    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));


            var dest = _mapper.Map<gridDto<TOUT>>(result);
            return dest;
        }

        public async Task<gridDto<TOUT>> GetAsync(DataSourceRequest dataSourceRequest)
        {
          
          
            //return await _unitofWork._context.Set<T>().ToDataSourceResultAsync(dataSourceRequest);
            DataSourceResult result = (await _unitofWork._context.Set<TIN>().AsNoTracking().ToDataSourceResultAsync(dataSourceRequest) as DataSourceResult);

            //genericoOrigen<TIN>.data = (result.Data as IList<TIN>);
            ////ConvertToDBEntity(result);
            //var ver2 = _mapper.Map<genericoDestino<TOUT>>(result.Data);
            //var ver = _mapper.Map((genericoOrigen<TIN>.data),  typeof(TIN), typeof(TOUT)); // , typeof(genericoDestino<TOUT>))()
            var dest = _mapper.Map<gridDto<TOUT>>(result);
            return dest; // _mapper.Map<gridDto<CustomersDto>>(result);

            //return await _unitofWork._context.Set<T>().ToListAsync();
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
