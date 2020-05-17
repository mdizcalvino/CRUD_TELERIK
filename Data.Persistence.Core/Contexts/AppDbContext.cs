using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistence.Core
{
    public class AppDbContext : DbContext, IContextUnitOfWork
    {
        /// <summary>
        /// DCC: El contexto viene cargado por la inicialización de la clase Startup.cs => Services => WebApi
        /// </summary>
        public AppDbContext() 
        {
        }

        DbSet<Persona> _persona;
        DbSet<Usuario> _usuario;
        
        public DbSet<Persona> Persona {
        get { return _persona ?? (_persona = base.Set<Persona>()); }
        }

        public DbSet<Usuario> Usuario
        {
            get { return _usuario ?? (_usuario = base.Set<Usuario>());  }
        }


        #region
        public  new DbSet<Entidad> Set<Entidad>() where Entidad : class
        {
            return base.Set<Entidad>();
        }
        public void Attach<Entidad>(Entidad item) where Entidad : class
        {
            if (Entry(item).State == EntityState.Detached)
            {
                base.Set<Entidad>().Attach(item);
            }
        }
        public void SetModified<Entidad>(Entidad item) where Entidad : class
        {
            Entry(item).State = EntityState.Modified;
        }
        public void SetDeleted<Entidad>(Entidad item) where Entidad : class
        {
            Entry(item).State = EntityState.Deleted;
        }


        public void Commit()
        {
            base.SaveChanges();
        }
        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Quitar la Eliminación en cascada
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }            
        }


    }
}
