using DAL.Context;
using DAL.Entities;
using DAL.Repository.Interfaces;
using EntityFramework.Filters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly GameShopContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(
            GameShopContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter).Where(x=>x.IsDeleted==false);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async Task<T> GetAsync(object id, string includeProperties = "")
        {
            if (string.IsNullOrEmpty(includeProperties))
            {
                return await _context.Set<T>().FindAsync(id);
            }

            var result = await _context.Set<T>().FindAsync(id);

            IQueryable<T> set = _context.Set<T>();

            set = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(set, (current, includeProperty)
                        => current.Include(includeProperty));

            return await set.FirstOrDefaultAsync(entity => entity == result);
        }

        public virtual void Insert(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public virtual void Delete(T entityToDelete)
        {
            entityToDelete.IsDeleted = true;
            Update(entityToDelete);
        }

        public virtual void Update(T entityToUpdate)
        {
            if (_context.Entry(entityToUpdate).State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entityToUpdate);
            }

            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
