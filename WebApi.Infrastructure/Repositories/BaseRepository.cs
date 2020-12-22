using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebApi.Core.Helpers;
using WebApi.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace WebApi.Infrastructure.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly DbContext _dbContext;

        protected BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual TEntity Add(TEntity entity)
        {
            return _dbContext.Set<TEntity>().Add(entity).Entity;
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            _dbContext.Set<TEntity>().AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            if (entity == null)
                return;

            _dbContext.Set<TEntity>().Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            if (entities == null || !entities.Any()) return;
            _dbContext.Set<TEntity>().UpdateRange(entities);
        }


        public virtual void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            if (entities == null || !entities.Any()) return;
            _dbContext.Set<TEntity>().RemoveRange(entities);
        }

        public virtual TEntity GetById(int id)
        {
            return _dbContext.Set<TEntity>().Find(id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().AsEnumerable<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetAll<TProperty>(Expression<Func<TEntity, TProperty>> include)
        {
            return _dbContext.Set<TEntity>().Include(include).AsEnumerable<TEntity>();
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> where)
        {
            return _dbContext.Set<TEntity>().FirstOrDefault(where);
        }

        public virtual TEntity Get<TProperty>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TProperty>> include)
        {
            return _dbContext.Set<TEntity>().Include(include).FirstOrDefault(where);
        }

        public virtual TEntity Get<TProperty>(Expression<Func<TEntity, bool>> where, List<Expression<Func<TEntity, TProperty>>> includes)
        {
            return CreateQuery(new List<Expression<Func<TEntity, bool>>>() { where }, includes, false).FirstOrDefault();
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> where, string includeNavigationPropertyPath)
        {
            return _dbContext.Set<TEntity>().Include(includeNavigationPropertyPath).FirstOrDefault(where);
        }

        public virtual IEnumerable<TEntity> GetMany(Expression<Func<TEntity, bool>> where, bool isListTrackingEnabled = false)
        {
            IQueryable<TEntity> result = _dbContext.Set<TEntity>().Where(where);
            return isListTrackingEnabled ? result : result.AsNoTracking();
        }

        public virtual IEnumerable<TEntity> GetMany<TProperty>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TProperty>> include, bool isListTrackingEnabled = false)
        {
            IQueryable<TEntity> result = _dbContext.Set<TEntity>().Include(include).Where(where);
            return isListTrackingEnabled ? result : result.AsNoTracking();
        }

        public virtual IEnumerable<TEntity> GetMany<TProperty>(Expression<Func<TEntity, bool>> where, List<Expression<Func<TEntity, TProperty>>> includes = null, bool isListTrackingEnabled = false)
        {
            return CreateQuery(new List<Expression<Func<TEntity, bool>>>() { where }, includes, isListTrackingEnabled);
        }

        public virtual IEnumerable<TEntity> GetMany<TProperty>(List<Expression<Func<TEntity, bool>>> where, List<Expression<Func<TEntity, TProperty>>> includes = null, bool isListTrackingEnabled = false)
        {
            return CreateQuery(where, includes, isListTrackingEnabled);
        }

        public virtual IEnumerable<TEntity> GetManyOrderedBy<TKey, TProperty>(List<Expression<Func<TEntity, bool>>> where
            , Expression<Func<TEntity, TKey>> keySelector = null
            , bool descending = false
            , List<Expression<Func<TEntity, TProperty>>> includes = null
            , bool isListTrackingEnabled = false)
        {
            IQueryable<TEntity> query = CreateQuery(where, includes, isListTrackingEnabled);
            return keySelector == null ? query.ToList() : (descending ? query.OrderByDescending(keySelector).ToList() : query.OrderBy(keySelector).ToList());
        }

        public virtual IEnumerable<TEntity> GetManyOrderedBy<TKey, TProperty>(Expression<Func<TEntity, bool>> where
            , Expression<Func<TEntity, TKey>> keySelector = null
            , bool descending = false
            , List<Expression<Func<TEntity, TProperty>>> includes = null
            , bool isListTrackingEnabled = false)
        {
            return GetManyOrderedBy(where == null ? null : new List<Expression<Func<TEntity, bool>>> { where }, keySelector, descending, includes, isListTrackingEnabled);
        }

        public virtual PagedList<TEntity> GetManyOrderedByWithPaging<TKey, TProperty>(Expression<Func<TEntity, bool>> where
            , int page
            , int pageSize
            , Expression<Func<TEntity, TKey>> keySelector = null
            , bool descending = false, List<Expression<Func<TEntity, TProperty>>> includes = null
            , bool isListTrackingEnabled = false)
        {
            IQueryable<TEntity> query = CreateQuery(new List<Expression<Func<TEntity, bool>>> { where }, includes, isListTrackingEnabled);

            if (keySelector != null)
                query = descending ? query.OrderByDescending(keySelector) : query.OrderBy(keySelector);

            return query.GetPaged(page, pageSize);
        }

        public virtual IEnumerable<TProperty> GetManyWithSelect<TProperty>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TProperty>> select, bool isListTrackingEnabled = false)
        {
            _dbContext.ChangeTracker.QueryTrackingBehavior = isListTrackingEnabled ? QueryTrackingBehavior.TrackAll
                                                                                    : QueryTrackingBehavior.NoTracking;
            return _dbContext.Set<TEntity>().Where(where).Select(select);
        }

        protected IQueryable<TEntity> CreateQuery<TProperty>(List<Expression<Func<TEntity, bool>>> where, List<Expression<Func<TEntity, TProperty>>> includes, bool isListTrackingEnabled)
        {
            DbSet<TEntity> dbSet = _dbContext.Set<TEntity>();
            IQueryable<TEntity> query = isListTrackingEnabled ? dbSet : dbSet.AsNoTracking();
            if (includes != null && includes.Count > 0)
            {
                foreach (Expression<Func<TEntity, TProperty>> inc in includes)
                    query = query.Include(inc);
            }
            if (where != null)
            {
                foreach (Expression<Func<TEntity, bool>> exp in where)
                    query = query.Where(exp);
            }
            return query;
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> where)
        {
            return _dbContext.Set<TEntity>().FirstOrDefault(where);
        }

        public void EnableTracking()
        {
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
        }

        public void DisableTracking()
        {
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _dbContext.Database.BeginTransaction();
        }
    }
}
