using BankSystem.Data;
using BankSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BankSystem.DataAccess.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where  TEntity : BaseEntity
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

        public virtual void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }
        public virtual void Update(TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
