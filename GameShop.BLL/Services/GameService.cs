using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using GameShop.BLL.DTO.FilterDTOs;
using GameShop.BLL.DTO.GameDTOs;
using GameShop.BLL.DTO.PaginationDTOs;
using GameShop.BLL.Enums;
using GameShop.BLL.Enums.Extensions;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Filters.Interfaces;
using GameShop.BLL.Pagination.Extensions;
using GameShop.BLL.Pipelines;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.BLL.Strategies.Interfaces.Factories;
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
        private readonly IFiltersFactory<IQueryable<Game>> _filtersFactory;
        private readonly IGameSortingFactory _gameSortingFactory;

        public GameService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoggerManager loggerManager,
            IValidator<GameCreateDTO> validator,
            IFiltersFactory<IQueryable<Game>> filtersFactory,
            IGameSortingFactory gameSortingFactory)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggerManager = loggerManager;
            _validator = validator;
            _filtersFactory = filtersFactory;
            _gameSortingFactory = gameSortingFactory;
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
            var game = (await _unitOfWork.GameRepository.GetAsync(
                filter: g => g.Key == gameKey, includeProperties: "GamePlatformTypes,GameGenres,Publisher"))
                    .SingleOrDefault();

            if (game == null)
            {
                throw new NotFoundException($"Game with key {gameKey} not found");
            }

            game.Views += 1;
            var model = _mapper.Map<GameReadDTO>(game);

            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.SaveAsync();

            _loggerManager.LogInfo($"Game with key {gameKey} returned successfully");
            return model;
        }

        public async Task<PagedListViewModel<GameReadListDTO>> GetAllGamesAsync(GameFiltersDTO gameFiltersDTO)
        {
            var query = _unitOfWork.GameRepository.GetQuery(
                filter: null,
                includeProperties: "GameGenres,Comments,GamePlatformTypes,Publisher");

            var pipeline = new GameFiltersPipeline();
            pipeline.Register(_filtersFactory.GetOperation(gameFiltersDTO));
            query = pipeline.PerformOperation(query);

            if (!string.IsNullOrEmpty(gameFiltersDTO.SortingOption))
            {
                var sortingType = gameFiltersDTO.SortingOption.ToEnum<SortingTypes>();
                var sortingStrategy = _gameSortingFactory.GetGamesSortingStrategy(sortingType);
                query = sortingStrategy.Sort(query);
            }

            var games = await query.ToListAsync();
            var pagedGames = games.ToPagedList(gameFiltersDTO.PageNumber, gameFiltersDTO.PageSize);
            var pagedModels = _mapper.Map<PagedListViewModel<GameReadListDTO>>(pagedGames);

            _loggerManager.LogInfo($"Games successfully returned with array size of {pagedModels.Entities.Count()}");
            return pagedModels;
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

        public async Task<int> GetNumberOfGamesAsync()
        {
            var count = await _unitOfWork.GameRepository.GetCountAsync();
            return count;
        }
    }
}
