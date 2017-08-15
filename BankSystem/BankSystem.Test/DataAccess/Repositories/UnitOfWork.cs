
using BankSystem.Test.Data;
using BankSystem.Test.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.Test.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BankSystemContext _dbContext;
        #region Repositories
        public IRepository<Transaction> TransactionRepository =>
           new GenericRepository<Transaction>(_dbContext);
        public IRepository<User> UserRepository =>
           new GenericRepository<User>(_dbContext);
        #endregion
        public UnitOfWork(BankSystemContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Commit()
        {
            _dbContext.SaveChanges();
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }
        public void RejectChanges()
        {
            foreach (var entry in _dbContext.ChangeTracker.Entries()
                  .Where(e => e.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }
    }
}
