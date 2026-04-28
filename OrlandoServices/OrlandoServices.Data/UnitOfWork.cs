using Microsoft.EntityFrameworkCore.Storage;
using OrlandoServices.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DBContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(DBContext context)
        {
            _context = context;
        }

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _transaction?.Commit();
            _transaction?.Dispose();
            _transaction = null;
        }

        public void Rollback()
        {
            _transaction?.Rollback();
            _transaction?.Dispose();
            _transaction = null;
        }
    }
}
