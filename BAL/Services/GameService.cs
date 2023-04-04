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

        public async Task Create(Game game, IEnumerable<int> gameGenres, IEnumerable<int> gamePlatformTypes)
        {
            foreach(var genre in gameGenres)
            {
                var genreToAdd = await _genreService.GetByIdAsync(genre);
                game.GameGenres.Add(genreToAdd);
            }
            foreach(var glt in gamePlatformTypes)
            {
                var pltToAdd = await _platformTypeService.GetByIdAsync(glt);
                game.GamePlatformTypes.Add(pltToAdd);
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

        public async Task<Game> GetByIdAsync(int id)
        {
            var game = await _gameRepository.GetByIdAsync(id, 
                includeProperties: "GameGenres.Genre,GamePlatformTypes.PlatformType");

            if(game == null)
            {
                throw new NotFoundException();
            }

            return game;
        }

        public async Task<IEnumerable<Game>> GetAsync(string search = "")
        {
            var filter = GetFilterQuery(search);
            var games = await _gameRepository.GetAsync(filter:filter, includeProperties: "GameGenres.Genre,GamePlatformTypes.PlatformType");

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

        public async Task<Stream> GenerateGameFile(int id)
        {
            var gameToDownload = await GetByIdAsync(id);

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

            filterQuery = u => u.Key.ToLower().Contains(formattedFilter);

            return filterQuery;
        }
    }
}
