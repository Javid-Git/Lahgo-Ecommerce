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
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, params string[] includes);
        Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> expression);
        void Remove(TEntity entity);
    }
}
