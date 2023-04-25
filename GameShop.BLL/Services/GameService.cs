using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using GameShop.BLL.DTO.GameDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;

namespace GameShop.BLL.Services
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
        private readonly IValidator<GameCreateDTO> _validator;

        public GameService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoggerManager loggerManager,
            IValidator<GameCreateDTO> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggerManager = loggerManager;
            _validator = validator;
        }

        public async Task CreateAsync(GameCreateDTO newGameDTO)
        {
            await _validator.ValidateAndThrowAsync(newGameDTO);

            var gameToAdd = _mapper.Map<Game>(newGameDTO);

            var allGenres = await _unitOfWork.GenreRepository.GetAsync(
                filter: g => newGameDTO.GenresId.Contains(g.Id));

            var allPlatformTypes = await _unitOfWork.PlatformTypeRepository.GetAsync(
                filter: plt => newGameDTO.PlatformTypeId.Contains(plt.Id));

            foreach (var genreId in newGameDTO.GenresId)
            {
                var genreToAdd = allGenres.SingleOrDefault(g => g.Id == genreId);

                if (genreToAdd == null)
                {
                    throw new NotFoundException($"Genre with id {genreId} not found");
                }

                gameToAdd.GameGenres.Add(genreToAdd);
            }

            foreach (var platformTypeId in newGameDTO.PlatformTypeId)
            {
                var platformTypeToAdd = allPlatformTypes.SingleOrDefault(plt => plt.Id == platformTypeId);

                if (platformTypeToAdd == null)
                {
                    throw new NotFoundException($"Platform type with id {platformTypeId} not found");
                }

                gameToAdd.GamePlatformTypes.Add(platformTypeToAdd);
            }

            _unitOfWork.GameRepository.Insert(gameToAdd);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Game with key {newGameDTO.Key} created successfully");
        }

        public async Task DeleteAsync(string gameKey)
        {
            var gamesToDelete = await _unitOfWork.GameRepository.GetAsync(
                filter: g => g.Key == gameKey);

            var gameToDelete = gamesToDelete.SingleOrDefault();

            if (gameToDelete == null)
            {
                throw new NotFoundException($"Game with key {gameKey} not found");
            }

            _unitOfWork.GameRepository.Delete(gameToDelete);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Game with key {gameToDelete.Key} deleted successfully");
        }

        public async Task<GameReadDTO> GetGameByKeyAsync(string gameKey)
        {
            var game = await _unitOfWork.GameRepository.GetAsync(
                filter: g => g.Key == gameKey, includeProperties: "GamePlatformTypes,GameGenres,Publisher");

            if (game.SingleOrDefault() == null)
            {
                throw new NotFoundException($"Game with key {gameKey} not found");
            }

            var model = _mapper.Map<GameReadDTO>(game.SingleOrDefault());
            _loggerManager.LogInfo($"Game with key {gameKey} returned successfully");
            return model;
        }

        public async Task<IEnumerable<GameReadListDTO>> GetAllGamesAsync()
        {
            var games = await _unitOfWork.GameRepository.GetAsync();

            var models = _mapper.Map<IEnumerable<GameReadListDTO>>(games);

            _loggerManager.LogInfo($"Games successfully returned with array size of {models.Count()}");
            return models;
        }

        public async Task<IEnumerable<GameReadListDTO>> GetGamesByGenreAsync(int genreId)
        {
            var games = await _unitOfWork.GameRepository.GetAsync(
                filter: g => g.GameGenres.Any(gg => gg.Id == genreId));

            var models = _mapper.Map<IEnumerable<GameReadListDTO>>(games);

            _loggerManager.LogInfo(
                $"Games with genreId {genreId} successfully returned with array size of {models.Count()}");
            return models;
        }

        public async Task<IEnumerable<GameReadListDTO>> GetGamesByPlatformTypeAsync(int platformTypeId)
        {
            var games = await _unitOfWork.GameRepository.GetAsync(filter:
                    g => g.GamePlatformTypes.Any(gg => gg.Id == platformTypeId));

            var models = _mapper.Map<IEnumerable<GameReadListDTO>>(games);

            _loggerManager.LogInfo(
                $"Games with platformTypeId {platformTypeId} successfully returned with array size of {models.Count()}");
            return models;
        }

        public async Task UpdateAsync(GameUpdateDTO updatedGameDTO)
        {
            await _validator.ValidateAndThrowAsync(updatedGameDTO);

            var exGame = (await _unitOfWork.GameRepository.GetAsync(
                filter: game => game.Key == updatedGameDTO.Key,
                includeProperties: "GameGenres,GamePlatformTypes")).SingleOrDefault();

            _mapper.Map(updatedGameDTO, exGame);

            if (exGame == null)
            {
                throw new BadRequestException($"Game with key {updatedGameDTO.Key} not found");
            }

            exGame.GamePlatformTypes.Clear();
            exGame.GameGenres.Clear();

            var allGenres = await _unitOfWork.GenreRepository.GetAsync(
                filter: g => updatedGameDTO.GenresId.Contains(g.Id));

            var allPlatformTypes = await _unitOfWork.PlatformTypeRepository.GetAsync(
                filter: plt => updatedGameDTO.PlatformTypeId.Contains(plt.Id));

            foreach (var genreId in updatedGameDTO.GenresId)
            {
                var genreToAdd = allGenres.SingleOrDefault(g => g.Id == genreId);

                if (genreToAdd == null)
                {
                    throw new NotFoundException($"Genre with id {genreId} not found");
                }

                exGame.GameGenres.Add(genreToAdd);
            }

            foreach (var platformTypeId in updatedGameDTO.PlatformTypeId)
            {
                var platformTypeToAdd = allPlatformTypes.SingleOrDefault(plt => plt.Id == platformTypeId);

                if (platformTypeToAdd == null)
                {
                    throw new NotFoundException($"PlatformType with id {platformTypeId} not found");
                }

                exGame.GamePlatformTypes.Add(platformTypeToAdd);
            }

            _unitOfWork.GameRepository.Update(exGame);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Game with key {updatedGameDTO.Key} updated successfully");
        }

        public async Task<MemoryStream> GenerateGameFileAsync(string key)
        {
            var game = (await _unitOfWork.GameRepository.GetAsync(filter: g => g.Key == key)).SingleOrDefault();

            if (game == null)
            {
                throw new NotFoundException($"Game with key {key} not found");
            }

            var dataToDownload = $"Game-{game.Name}|{game.Key}|{game.Description}";
            var stringInMemoryStream = new MemoryStream(Encoding.ASCII.GetBytes(dataToDownload));

            _loggerManager.LogInfo($"Upload data successfully created for game with key {game.Key}");
            return stringInMemoryStream;
        }

        public async Task<int> GetNumberOfGames()
        {
            var games = await _unitOfWork.GameRepository.GetAsync();

            return games.Count();
        }
    }
}
