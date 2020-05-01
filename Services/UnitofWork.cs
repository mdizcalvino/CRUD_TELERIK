using Modelos.Contexto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{

    public interface IUnitofWork : IDisposable
    {
        ApplicationDBContext _context { get; }
        void Commit();
    }

    public class UnitofWork : IUnitofWork
    {
        public ApplicationDBContext _context { get; }

        public UnitofWork(ApplicationDBContext context)
        {
            _context = context;
        }
        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
