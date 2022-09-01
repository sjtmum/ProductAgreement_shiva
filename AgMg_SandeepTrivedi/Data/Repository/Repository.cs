using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AgMg_SandeepTrivedi.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        bool Any(Expression<Func<T, bool>> expression);
        T Get(object id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Get(Expression<Func<T, bool>> expression);
        IEnumerable<T> Get(Expression<Func<T, bool>> expression, int pageNumber, int pageSize);
        IEnumerable<T> Get(Expression<Func<T, bool>> expression, string filter, string order, int pageNumber, int pageSize);
        IEnumerable<T> GetWithRawSql(string query, params object[] parameters);
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        T Add(T entity);
        IEnumerable<T> AddRange(IEnumerable<T> entities);
        T Update(T entity);
        void Remove(T entity);
        void Remove(object id);
        void RemoveRange(IEnumerable<T> entities);
        void Delete(T entity);
    }
    public class Repository<T> : IRepository<T> where T : class
    {
        internal DbSet<T> dbSet = null;
        internal DbContext dbContext = null;
        public Repository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }
        public bool Any(Expression<Func<T, bool>> expression)
        {
            return dbSet.Any(expression);
        }
        public T Add(T entity)
        {
            dbSet.Add(entity);
            return entity;
        }
        public IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
            return entities;
        }
        public IEnumerable<T> Get(Expression<Func<T, bool>> expression)
        {
            return dbSet.Where(expression).AsEnumerable<T>();
        }
        public IEnumerable<T> Get(Expression<Func<T, bool>> expression, int pageNumber, int pageSize)
        {
            return dbSet.Where(expression).Skip((pageNumber - 1) * pageSize).Take(pageSize).AsEnumerable<T>();
        }
        public IEnumerable<T> Get(Expression<Func<T, bool>> expression, string filter, string order, int pageNumber, int pageSize)
        {
            throw new Exception();
        }
        public IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
                query = query.Where(filter);
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            if (orderBy != null)
                return orderBy(query).ToList();
            else
                return query.ToList();
        }
        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.AsEnumerable<T>();
        }
        public T Get(object id)
        {
            return dbSet.Find(id);
        }
        public IEnumerable<T> GetWithRawSql(string query, params object[] parameters)
        {
            throw new NotImplementedException();
        }
        public void Remove(T entity)
        {
            T existing = dbSet.Find(entity);
            if (existing != null)
                dbSet.Remove(existing);
        }
        public void Remove(object id)
        {
            T existing = dbSet.Find(id);
            if (existing != null)
                dbSet.Remove(existing);
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
        public T Update(T entity)
        {
            dbSet.Update(entity);
            return entity;
        }
        public void Delete(T entity)
        {
            dbSet.Update(entity);
        }
    }
}
