using BAL.Services.Interfaces;
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
    public class GameService : IGameService
    {
        private readonly IRepository<Game> _gameRepository;

        public GameService(IUnitOfWork unitOfWork)
        {
            _gameRepository = unitOfWork.GameRepository;
        }

        public async Task Create(Game game)
        {
            _gameRepository.Insert(game);
            await _gameRepository.SaveChangesAsync();
        }

        public async Task Delete(object id)
        {
            var gameToDelete = await _gameRepository.GetAsync(id);
            _gameRepository.Delete(gameToDelete);
            await _gameRepository.SaveChangesAsync();

        }

        public async Task<IEnumerable<Game>> GetAsync()
        {
            var games = await _gameRepository.GetAsync();
            return games;
        }

        public Task<IEnumerable<Game>> GetAsync(string search)
        {
            var filter = GetFilterQuery(search);

            var games = _gameRepository.GetAsync(
                filter: filter,
                includeProperties: "Game.Coments,Game.GameGenres,Game.GamePlatformTypes");

            return games;
        }

        public async Task Update(Game game)
        {
            _gameRepository.Update(game);
            await _gameRepository.SaveChangesAsync();
        }

        private static Expression<Func<Game, bool>> GetFilterQuery(string filterParam)
        {
            Expression<Func<Game, bool>> filterQuery = null;

            if (filterParam is null) return filterQuery;

            var formattedFilter = filterParam.Trim().ToLower();

            filterQuery = u => u.Key.ToLower().Contains(formattedFilter)
                                    || u.GameGenres.All(g=>g.Name == formattedFilter)
                                    || u.GamePlatformTypes.All(plt => plt.Type == formattedFilter);

            return filterQuery;
        }
    }
}
