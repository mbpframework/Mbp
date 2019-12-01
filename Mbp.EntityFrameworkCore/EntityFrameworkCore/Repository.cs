using Mbp.Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mbp.EntityFrameworkCore.EntityFrameworkCore
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        private readonly ILogger _logger;

        public Repository()
        {

        }

        public bool CheckExists(Expression<Func<TEntity, bool>> predicate, TKey id = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckExistsAsync(Expression<Func<TEntity, bool>> predicate, TKey id = default)
        {
            throw new NotImplementedException();
        }

        public int Delete(params TEntity[] entities)
        {
            throw new NotImplementedException();
        }

        public int Delete(TKey key)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(params TEntity[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TKey key)
        {
            throw new NotImplementedException();
        }

        public int DeleteBatch(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteBatchAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public TEntity Get(TKey key)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetAsync(TKey key)
        {
            throw new NotImplementedException();
        }

        public TEntity GetFirst(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public TEntity GetFirst(Expression<Func<TEntity, bool>> predicate, bool filterByDataAuth)
        {
            throw new NotImplementedException();
        }

        public int Insert(params TEntity[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync(params TEntity[] entities)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> Query()
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate, bool filterByDataAuth)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includePropertySelectors)
        {
            throw new NotImplementedException();
        }

        public int Update(params TEntity[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(params TEntity[] entities)
        {
            throw new NotImplementedException();
        }

        public int UpdateBatch(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateBatchAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            throw new NotImplementedException();
        }
    }
}
