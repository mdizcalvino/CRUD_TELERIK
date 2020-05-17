using Modelos.Contexto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{

    public interface IUnitOfWork : IDisposable
    {
        ApplicationDBContext _context { get; }
        void Commit();
    }

    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDBContext _context { get; }

        public UnitOfWork(ApplicationDBContext context)
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
