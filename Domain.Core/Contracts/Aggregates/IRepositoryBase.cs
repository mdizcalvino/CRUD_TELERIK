using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Domain.Core
{
    /// <summary>
    /// Interface de Repositorio Generico
    /// </summary>
    /// <typeparam name="Entidad"></typeparam>
    public  interface IRepositoryBase<Entidad> : IDisposable
    {
        IUnitOfWork UnitOfWork { get; }

        Entidad GetById(object pId);

        IEnumerable<Entidad> GetByFilter(Expression<Func<Entidad, bool>> pFiltro);

        IEnumerable<Entidad> GetByFilter(Expression<Func<Entidad, bool>> pFiltro, string pPropertiesIncluded);

        IEnumerable<Entidad> GetAll();

        Entidad Create(Entidad pEntityCreate);

        void Delete(Entidad pEntityDelete);
    }
}
