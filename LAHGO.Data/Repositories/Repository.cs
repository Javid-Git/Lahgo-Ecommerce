using LAHGO.Core.Entities;
using LAHGO.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LAHGO.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly AppDbContext _context;
        public Repository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _context.Set<TEntity>().Where(expression).ToListAsync();
        }
        public  IQueryable<TEntity> GetAllAsyncQuery(Expression<Func<TEntity, bool>> expression)
        {
            return _context.Set<TEntity>().Where(expression).AsQueryable();
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression)
        {
            return  _context.Set<TEntity>().Where(expression).AsQueryable();
        }
        public async Task<List<TEntity>> GetAllAsyncInclude(Expression<Func<TEntity, bool>> expression, params string[] includes)
        {
            var queryable =  _context.Set<TEntity>().Where(expression);

            if (includes != null && includes.Length > 0)
            {
                foreach (string include in includes)
                {
                    queryable = queryable.Include(include);
                }
            }
            return await queryable.ToListAsync();
        }
        public IQueryable<TEntity> Include(params string[] includes)
        {
            IQueryable<TEntity> queryable = _context.Set<TEntity>();

            if (includes != null && includes.Length > 0)
            {
                foreach (string include in includes)
                {
                    queryable = queryable.Include(include);
                }
            }
            return  queryable;
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, params string[] includes)
        {
            var queryable = _context.Set<TEntity>().Where(expression);

            if (includes != null && includes.Length > 0)
            {
                foreach (string include in includes)
                {
                    queryable = queryable.Include(include);
                }
            }

            return await queryable.FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _context.Set<TEntity>().AnyAsync(expression);
        }

        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public async Task AddAllAsync(List<TEntity> entity)
        {
            await _context.Set<TEntity>().AddRangeAsync(entity);
        }

        public void RemoveAllAsync(List<TEntity> entity)
        {
            _context.Set<TEntity>().RemoveRange(entity);
        }
    }
}

