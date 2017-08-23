using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BankSystem.DataAccess.Repositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        void SaveChanges();
        IQueryable<TEntity> GetQuery();
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        int Count();
        bool Any(Expression<Func<TEntity, bool>> predicate);
        TEntity GetById(Guid id);
        bool Add(TEntity entity);
        bool Delete(TEntity entity);
        bool Update(TEntity entity);
    }
}