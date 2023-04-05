using BAL.Exceptions;
using BAL.Services.Interfaces;
using DAL.Entities;
using DAL.Models;
using DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class GameService : IGameService
    {
        private readonly IRepository<Game> _gameRepository;
        private readonly IRepository<Genre> _genreRepository;
        private readonly IRepository<PlatformType> _platformTypeRepository;

        public GameService(
            IUnitOfWork unitOfWork,
            IGenreService genreService,
            IPlatformTypeService platformTypeService)
        {
            _gameRepository = unitOfWork.GameRepository;
            _genreRepository = unitOfWork.GenreRepository;
            _platformTypeRepository = unitOfWork.PlatformTypeRepository;
        }

        public async Task Create(Game game, IEnumerable<int> gameGenres, IEnumerable<int> gamePlatformTypes)
        {
            foreach (var genre in gameGenres)
            {
                var genreToAdd = await _genreRepository.GetByIdAsync(genre);
                game.GameGenres.Add(genreToAdd);
            }
            foreach (var glt in gamePlatformTypes)
            {
                var gltToAdd = await _platformTypeRepository.GetByIdAsync(glt);
                game.GamePlatformTypes.Add(gltToAdd); 
            }

            _gameRepository.Insert(game);
            await _gameRepository.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var gameToDelete = await _gameRepository.GetByIdAsync(id);
            _gameRepository.Delete(gameToDelete);
            await _gameRepository.SaveChangesAsync();
        }

        public async Task<Game> GetByKeyGameAsync(string gameKey)
        {
            var game = await _gameRepository.GetAsync(filter:g=>g.Key==gameKey,includeProperties: "GamePlatformTypes,GameGenres");

            if(game == null)
            {
                throw new NotFoundException();
            }

            return game.SingleOrDefault();
        }

        public async Task<IEnumerable<Game>> GetAllGamesAsync()
        {
            var games = await _gameRepository.GetAsync();

            if (games == null)
            {
                throw new NotFoundException();
            }

            return games;
        }

        public async Task<IEnumerable<Game>> GetGameByGenreOrPltAsync(GameParameters gameParameters)
        {
            if (!string.IsNullOrEmpty(gameParameters.PlatformType))
            {
                var games = await _gameRepository.GetAsync(filter:
                    g => g.GamePlatformTypes.Any(gg => gg.Type == gameParameters.GenreName.Trim().ToLower()));

                if (games == null)
                {
                    throw new NotFoundException();
                }

                return games;
            }
            else if (!string.IsNullOrEmpty(gameParameters.GenreName))
            {
                var games = await _gameRepository.GetAsync(filter:
                    g => g.GameGenres.Any(gg => gg.Name == gameParameters.GenreName.Trim().ToLower()));

                if (games == null)
                {
                    throw new NotFoundException();
                }

                return games;
            }
            else
            {
                throw new BadRequestException();
            }
        }

        public async Task Update(Game game, IEnumerable<int> genresId, IEnumerable<int> platformTypesId)
        {
            var exGame = await _gameRepository.GetByIdAsync(game.Id, q => q.GameGenres, q => q.GamePlatformTypes);

            exGame.GamePlatformTypes.Clear();
            exGame.GameGenres.Clear();

            foreach (var genre in genresId)
            {
                var genreToAdd = await _genreRepository.GetByIdAsync(genre);
                exGame.GameGenres.Add(genreToAdd);
            }
            foreach (var glt in platformTypesId)
            {
                var gltToAdd = await _platformTypeRepository.GetByIdAsync(glt);
                exGame.GamePlatformTypes.Add(gltToAdd);
            }

            _gameRepository.Update(exGame);
            await _gameRepository.SaveChangesAsync();
        }

        public HttpResponseMessage GenerateGameFile(string path)
        {
            HttpResponseMessage res = new HttpResponseMessage(HttpStatusCode.OK);

            var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            res.Content = new StreamContent(stream);
            res.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octec-stream");

            return res;
        }
        private static Expression<Func<Game, bool>> GetFilterQuery(string filterParam = "", GameParameters gameParameters = null)
        {
            Expression<Func<Game, bool>> filterQuery = null;

            if (!string.IsNullOrEmpty(filterParam))
            {
                var formattedFilter = filterParam.Trim().ToLower();

                filterQuery = u => u.Key.ToLower().Contains(formattedFilter);

                return filterQuery;
            }
            else if (gameParameters != null)
            {
                if (!string.IsNullOrEmpty(gameParameters.PlatformType))
                {
                    var formatedFilter = gameParameters.PlatformType.Trim().ToLower();
                    filterQuery = u => u.GamePlatformTypes.Any(g=>g.Type == formatedFilter);
                    return filterQuery;
                }
                else if (!string.IsNullOrEmpty(gameParameters.GenreName))
                {
                    var formatedFilter = gameParameters.PlatformType.Trim().ToLower();
                    filterQuery = u => u.GameGenres.Any(g => g.Name == formatedFilter);
                    return filterQuery;
                }
            }
            return filterQuery;
        }
    }
}
