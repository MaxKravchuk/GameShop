using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameShop.DAL.Entities;

namespace GameShop.DAL.Repository.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetQuery(
              Expression<Func<T, bool>> filter,
              Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
              string includeProperties = "");

        Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            bool asNoTracking = false);

        Task<T> GetByIdAsync(int id, string includeProperties = "");

        void Insert(T entity);

        void Delete(T entityToDelete);

        void Update(T entityToUpdate);

        Task<int> GetCountAsync();
    }
}
