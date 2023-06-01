using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameShop.DAL.Context;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;

namespace GameShop.DAL.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly GameShopContext _context;

        public Repository(GameShopContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetQuery(
            Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> set = filter == null ? _context.Set<T>().Where(x => !x.IsDeleted)
                : _context.Set<T>().Where(filter).Where(x => !x.IsDeleted);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                set = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(set, (current, includeProperty)
                        => current.Include(includeProperty));
            }

            if (orderBy != null)
            {
                set = orderBy(set);
            }

            return set;
        }

        public async Task<IEnumerable<T>> GetAsync(
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

            foreach (var includeProperty in includeProperties.Split(
                new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                set = set.Include(includeProperty);
            }

            return await set.FirstOrDefaultAsync(en => en == result && en.IsDeleted == false);
        }

        public void Insert(T entity)
        {
            var x = _context.Set<T>().Add(entity);
        }

        public void Delete(T entityToDelete)
        {
            entityToDelete.IsDeleted = true;
            Update(entityToDelete);
        }

        public void HardDelete(T entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entityToDelete);
            }

            _context.Set<T>().Remove(entityToDelete);
        }

        public void Update(T entityToUpdate)
        {
            if (_context.Entry(entityToUpdate).State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entityToUpdate);
            }

            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public async Task<int> GetCountAsync()
        {
            return await GetQuery(filter: null).CountAsync();
        }
    }
}
