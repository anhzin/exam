
using BankSystem.Test.Data;
using BankSystem.Test.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BankSystem.Test.DataAccess.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly BankSystemContext Context;
        public GenericRepository(BankSystemContext dbContext)
        {
            Context = dbContext;
        }


        public virtual void SaveChanges()
        {
            Context.SaveChanges();
        }

        public virtual IQueryable<TEntity> GetQuery()
        {
            return Context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return GetQuery().Where(predicate).ToList();
        }

        public virtual int Count()
        {
            return GetQuery().Count();
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return GetQuery().Any(predicate);
        }

        public virtual TEntity GetById(Guid id)
        {
            return Context.Set<TEntity>().FirstOrDefault(entity => entity.ID == id);
        }

        public virtual bool Add(TEntity entity)
        {
            try
            {
                Context.Set<TEntity>().Add(entity);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public virtual bool Delete(TEntity entity)
        {
            try
            {
                Context.Set<TEntity>().Remove(entity);
                return true;
            }
            catch
            {
                return false;
            }

        }
        public virtual bool Update(TEntity entity)
        {
            try
            {
                Context.Set<TEntity>().Update(entity);
                return true;
            }
            catch
            {
                return false;
            }


        }
        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}
