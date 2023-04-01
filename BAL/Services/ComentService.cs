using BAL.Services.Interfaces;
using BAL.ViewModels.ComentViewModels;
using DAL.Entities;
using DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class ComentService : IComentService
    {
        private readonly IRepository<Coment> _comentRepository;

        public ComentService(IUnitOfWork unitOfWork)
        {
            _comentRepository = unitOfWork.ComentRepository;
        }

        public async Task Create(Coment coment)
        {
            _comentRepository.Insert(coment);
            await _comentRepository.SaveChangesAsync();
        }

        public async Task Delete(object id)
        {
            var comentToDelete = await _comentRepository.GetAsync(id);
            _comentRepository.Delete(comentToDelete);
            await _comentRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Coment>> GetAsync()
        {
            var coments = await _comentRepository.GetAsync();
            return coments;
        }
        public async Task<IEnumerable<Coment>> GetAsync(string gameKey)
        {
            var filter = GetFilterQuery(gameKey);

            var coments = await _comentRepository.GetAsync(filter: filter);
            return coments;
        }
        public async Task<Coment> GetAsync(int comentId)
        {
            var coment = await _comentRepository.GetAsync(comentId);
            return coment;
        }

        public async Task Update(Coment coment)
        {
            _comentRepository.Update(coment);
            await _comentRepository.SaveChangesAsync();
        }

        private static Expression<Func<Coment, bool>> GetFilterQuery(string filterParam)
        {
            Expression<Func<Coment, bool>> filterQuery = null;

            if (filterParam is null) return filterQuery;

            var formattedFilter = filterParam.Trim().ToLower();

            filterQuery = u => u.GameKey.ToLower().Contains(formattedFilter);

            return filterQuery;
        }
    }
}
