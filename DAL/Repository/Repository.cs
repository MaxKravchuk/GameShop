using DAL.Context;
using DAL.Entities;
using DAL.Repository.Interfaces;
using EntityFramework.Filters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
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

        public IQueryable<T> GetQuery(
            Expression<Func<T, bool>> filte,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> set = filte == null ? _context.Set<T>()
                : _context.Set<T>().Where(filte).Where(x=>x.IsDeleted==false);

            if(!string.IsNullOrEmpty(includeProperties))
            {
                set = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(set, (current, includeProperty)
                        => current.Include(includeProperty));
            }

            if(orderBy != null)
            {
                set = orderBy(set);
            }

            return set;
        }

        public virtual async Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            bool asNoTracking = false)
        {
            var query = GetQuery(filter, orderBy, includeProperties);

            if (asNoTracking)
            {
                var noTrackingResult = await query.AsNoTracking().ToListAsync();

                return noTrackingResult;
            }

            var trackingResult = await query.ToListAsync();
            return trackingResult;
        }

        public async Task<T> GetByIdAsync(int id, string includeProperties = "")
        {
            if (string.IsNullOrEmpty(includeProperties))
            {
                return await _context.Set<T>().FindAsync(id);
            }

            var result = await _context.Set<T>().FindAsync(id);
            
            IQueryable<T> set = _context.Set<T>();

            //set = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            //        .Aggregate(set, (current, includeProperty)
            //            => current.Include(includeProperty));

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                set = set.Include(includeProperty);
            }

            return await set.FirstOrDefaultAsync(en => en == result);
        }

        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            
            foreach(var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(x => x.Id == id);
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
