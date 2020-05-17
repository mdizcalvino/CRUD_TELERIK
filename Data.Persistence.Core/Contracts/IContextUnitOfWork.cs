using System;
using System.Collections.Generic;
using System.Text;
using Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistence.Core

{
   public interface IContextUnitOfWork : IUnitOfWork , IDisposable
    {
        #region Tablas de Proyecto
        DbSet<Persona> Persona { get;  }
        DbSet<Usuario> Usuario { get; }
        #endregion

        // Metodos de la interfaz        
        DbSet<Entidad> Set<Entidad>() where Entidad : class;
        void Attach<Entidad>(Entidad item) where Entidad : class;
        void SetModified<Entidad>(Entidad item) where Entidad : class;
        void SetDeleted<Entidad>(Entidad item) where Entidad : class;


    }
}
