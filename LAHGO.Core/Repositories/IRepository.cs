using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LAHGO.Core.Repositories 
{
    public interface IRepository<TEntity>
    {
        Task AddAsync(TEntity entity);
        Task AddAllAsync(List<TEntity> entity);
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression);
        IQueryable<TEntity> GetAllAsyncQuery(Expression<Func<TEntity, bool>> expression);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, params string[] includes);
        Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> expression);
        Task<List<TEntity>> GetAllAsyncInclude(Expression<Func<TEntity, bool>> expression, params string[] includes);
        void Remove(TEntity entity);
        IQueryable<TEntity> Include(params string[] includes);
        void RemoveAllAsync(List<TEntity> entity);
    }
}
