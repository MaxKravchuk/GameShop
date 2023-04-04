using BAL.Exceptions;
using BAL.Services.Interfaces;
using DAL.Entities;
using DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class GameService : IGameService
    {
        private readonly IRepository<Game> _gameRepository;
        private readonly IGenreService _genreService;
        private readonly IPlatformTypeService _platformTypeService;

        public GameService(
            IUnitOfWork unitOfWork,
            IGenreService genreService,
            IPlatformTypeService platformTypeService)
        {
            _gameRepository = unitOfWork.GameRepository;
            _genreService = genreService;
            _platformTypeService = platformTypeService;
        }

        public async Task Create(Game game, IEnumerable<string> gameGenres, IEnumerable<string> gamePlatformTypes)
        {
            foreach(var genre in gameGenres)
            {
                game.GameGenres.Add(new GameGenre
                {
                    Game = game,
                    Genre = await _genreService.GetByNameAsync(genre)
                }) ;
            }
            foreach(var glt in gamePlatformTypes)
            {
                game.GamePlatformTypes.Add(new GamePlatformType
                {
                    Game = game,
                    PlatformType = await _platformTypeService.GetByTypeAsync(glt)
                });
            }

            _gameRepository.Insert(game);
            await _gameRepository.SaveChangesAsync();
        }

        public async Task Delete(object id)
        {
            var gameToDelete = await _gameRepository.GetAsync(id);
            _gameRepository.Delete(gameToDelete);
            await _gameRepository.SaveChangesAsync();

        }

        public async Task<Game> GetByKeyAsync(object key)
        {
            var game = await _gameRepository.GetAsync(key, 
                includeProperties: "GameGenres.Genre,GamePlatformTypes.PlatformType,Coments");

            if(game == null)
            {
                throw new NotFoundException();
            }

            return game;
        }

        public async Task<IEnumerable<Game>> GetAsync(string search)
        {
            var filter = GetFilterQuery(search);

            var games = await _gameRepository.GetAsync(filter: filter);

            if (games == null)
            {
                throw new NotFoundException();
            }

            return games;
        }

        public async Task Update(Game game)
        {
            _gameRepository.Update(game);
            await _gameRepository.SaveChangesAsync();
        }

        public async Task<Stream> GenerateGameFile(object gameKey)
        {
            var gameToDownload = await GetByKeyAsync(gameKey);

            string fileName = $"{gameToDownload.Name}.bin";

            byte[] binareData = GenerateBinaryData(gameToDownload);

            using(Stream stream = new MemoryStream())
            {
                stream.Write(binareData, 0, binareData.Length);
                stream.Position = 0;
                stream.Close();

                return stream;
            }
        }

        private byte[] GenerateBinaryData(Game game)
        {
            byte[] binaryData = new byte[1024];
            for (int i = 0; i < binaryData.Length; i++)
            {
                binaryData[i] = (byte)(i % 256);
            }

            List<string> genresName = new List<string>();
            List<string> platformTypes = new List<string>();

            foreach (var genre in game.GameGenres)
            {
                genresName.Add(genre.Name);
            }
            foreach (var platformType in game.GamePlatformTypes)
            {
                platformTypes.Add(platformType.Type);
            }

            // Convert lists to arrays
            char[] genreChars = String.Concat(genresName).ToCharArray();
            char[] platformChars = String.Concat(platformTypes).ToCharArray();
            // Write game details to binary data
            byte[] gameNameBytes = Encoding.UTF8.GetBytes(game.Name);
            byte[] gameGenreBytes = Encoding.UTF8.GetBytes(genreChars);
            byte[] gamePlatformBytes = Encoding.UTF8.GetBytes(platformChars);

            int offset = 0;
            Buffer.BlockCopy(gameNameBytes, 0, binaryData, offset, gameNameBytes.Length);
            offset += gameNameBytes.Length;
            Buffer.BlockCopy(gameGenreBytes, 0, binaryData, offset, gameGenreBytes.Length);
            offset += gameGenreBytes.Length;
            Buffer.BlockCopy(gamePlatformBytes, 0, binaryData, offset, gamePlatformBytes.Length);

            return binaryData;
        }

        private static Expression<Func<Game, bool>> GetFilterQuery(string filterParam)
        {
            Expression<Func<Game, bool>> filterQuery = null;

            if (filterParam is null) return filterQuery;

            var formattedFilter = filterParam.Trim().ToLower();

            filterQuery = u => u.Name.ToLower().Contains(formattedFilter)
                                    || u.GameGenres.All(g=>g.Name == formattedFilter)
                                    || u.GamePlatformTypes.All(plt => plt.Type == formattedFilter);

            return filterQuery;
        }
    }
}
