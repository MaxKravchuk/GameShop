using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
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
using GameShop.DAL.Enums;
using GameShop.DAL.Repository.Interfaces;
using GameShop.DAL.Repository.Interfaces.Utils;

namespace GameShop.BLL.Services
{
    public class GameService : IGameService
    {
        private readonly string _defaultPhotoPath =
            Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data"), "default-image.jpg");

        private readonly IUnitOfWork _unitOfWork;
        private readonly IStoredProceduresProvider _storedProceduresProvider;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
        private readonly IValidator<GameCreateDTO> _validator;
        private readonly IFiltersFactory<IQueryable<Game>> _filtersFactory;
        private readonly IGameSortingFactory _gameSortingFactory;
        private readonly IBlobStorageProvider _blobStorageProvider;

        public GameService(
            IUnitOfWork unitOfWork,
            IStoredProceduresProvider storedProceduresProvider,
            IMapper mapper,
            ILoggerManager loggerManager,
            IValidator<GameCreateDTO> validator,
            IFiltersFactory<IQueryable<Game>> filtersFactory,
            IGameSortingFactory gameSortingFactory,
            IBlobStorageProvider blobStorageProvider)
        {
            _unitOfWork = unitOfWork;
            _storedProceduresProvider = storedProceduresProvider;
            _mapper = mapper;
            _loggerManager = loggerManager;
            _validator = validator;
            _filtersFactory = filtersFactory;
            _gameSortingFactory = gameSortingFactory;
            _blobStorageProvider = blobStorageProvider;
        }

        public async Task CreateAsync(GameCreateDTO newGameDTO)
        {
            await _validator.ValidateAndThrowAsync(newGameDTO);

            var gameToAdd = _mapper.Map<Game>(newGameDTO);

            var allGenres = newGameDTO.GenresId == null ?
                    new List<Genre>() :
                    await _unitOfWork.GenreRepository
                        .GetAsync(filter: g => newGameDTO.GenresId.Contains(g.Id));

            var allPlatformTypes = newGameDTO.PlatformTypeId == null ?
                new List<PlatformType>() :
                await _unitOfWork.PlatformTypeRepository
                    .GetAsync(filter: plt => newGameDTO.PlatformTypeId.Contains(plt.Id));

            foreach (var genre in allGenres)
            {
                gameToAdd.GameGenres.Add(genre);
            }

            foreach (var platformType in allPlatformTypes)
            {
                gameToAdd.GamePlatformTypes.Add(platformType);
            }

            var bytes = GetImageBytes(newGameDTO.PhotoUrl);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                var image = Image.FromStream(ms);
                gameToAdd.PhotoUrl =
                await _blobStorageProvider.UploadAsync(image, newGameDTO.Key, BlobContainerItemTypes.GamePictures);
            }

            _unitOfWork.GameRepository.Insert(gameToAdd);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Game with key {newGameDTO.Key} created successfully");
        }

        public async Task DeleteAsync(string gameKey)
        {
            var gameToDelete = await _storedProceduresProvider.GetGameByKeyAsync(gameKey);

            if (gameToDelete == null)
            {
                throw new NotFoundException($"Game with key {gameKey} not found");
            }

            await _blobStorageProvider.DeleteAsync(gameToDelete.PhotoUrl, BlobContainerItemTypes.GamePictures);

            _unitOfWork.GameRepository.Delete(gameToDelete);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Game with key {gameToDelete.Key} deleted successfully");
        }

        public async Task<GameReadDTO> GetGameByKeyAsync(string gameKey)
        {
            var game = await _unitOfWork.GameRepository.GetPureQuery(
                filter: g => g.Key == gameKey, includeProperties: "GamePlatformTypes,GameGenres,Publisher")
                .SingleOrDefaultAsync();

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

        public async Task<PagedListDTO<GameReadListDTO>> GetAllGamesAsync(GameFiltersDTO gameFiltersDTO)
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
            var pagedModels = _mapper.Map<PagedListDTO<GameReadListDTO>>(pagedGames);

            _loggerManager.LogInfo($"Games successfully returned with array size of {pagedModels.Entities.Count()}");
            return pagedModels;
        }

        public async Task<IEnumerable<GameReadListDTO>> GetGamesByGenreAsync(int genreId)
        {
            var games = await _storedProceduresProvider.GetGameByGenreIdAsync(genreId);

            var models = _mapper.Map<IEnumerable<GameReadListDTO>>(games);

            _loggerManager.LogInfo(
                $"Games with genreId {genreId} successfully returned with array size of {models.Count()}");
            return models;
        }

        public async Task<IEnumerable<GameReadListDTO>> GetGamesByPlatformTypeAsync(int platformTypeId)
        {
            var games = await _storedProceduresProvider.GetGameByPlatformTypeIdAsync(platformTypeId);

            var models = _mapper.Map<IEnumerable<GameReadListDTO>>(games);

            _loggerManager.LogInfo(
                $"Games with platformTypeId {platformTypeId} successfully returned with array size of {models.Count()}");
            return models;
        }

        public async Task<IEnumerable<GameReadListDTO>> GetGamesByPublisherAsync(int publisherId)
        {
            var games = await _unitOfWork.GameRepository.GetAsync(
                filter: g => g.PublisherId == publisherId,
                includeProperties: "Publisher");

            var models = _mapper.Map<IEnumerable<GameReadListDTO>>(games);

            _loggerManager.LogInfo(
                $"Games with publisher Id {publisherId} successfully returned with array size of {models.Count()}");
            return models;
        }

        public async Task UpdateAsync(GameUpdateDTO updatedGameDTO)
        {
            await _validator.ValidateAndThrowAsync(updatedGameDTO);

            var exGame = (await _unitOfWork.GameRepository.GetAsync(
                filter: game => game.Key == updatedGameDTO.Key,
                includeProperties: "GameGenres,GamePlatformTypes")).SingleOrDefault();

            if (updatedGameDTO.PhotoUrl != exGame.PhotoUrl)
            {
                var bytes = GetImageBytes(updatedGameDTO.PhotoUrl);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    var image = Image.FromStream(ms);
                    updatedGameDTO.PhotoUrl = await _blobStorageProvider
                    .UpdateAsync(image, exGame.PhotoUrl, updatedGameDTO.Name, BlobContainerItemTypes.GamePictures);
                }
            }

            _mapper.Map(updatedGameDTO, exGame);

            if (exGame == null)
            {
                throw new BadRequestException($"Game with key {updatedGameDTO.Key} not found");
            }

            exGame.GamePlatformTypes.Clear();
            exGame.GameGenres.Clear();

            var allGenres = updatedGameDTO.GenresId == null ?
                    new List<Genre>() :
                    await _unitOfWork.GenreRepository
                        .GetAsync(filter: g => updatedGameDTO.GenresId.Contains(g.Id));

            var allPlatformTypes = updatedGameDTO.PlatformTypeId == null ?
                new List<PlatformType>() :
                await _unitOfWork.PlatformTypeRepository
                    .GetAsync(filter: plt => updatedGameDTO.PlatformTypeId.Contains(plt.Id));

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
            var game = await _storedProceduresProvider.GetGameByKeyAsync(key);

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

        private byte[] GetImageBytes(string photoUrl)
        {
            byte[] bytes = File.ReadAllBytes(_defaultPhotoPath);
            if (photoUrl != "Empty")
            {
                bytes = Convert.FromBase64String(photoUrl);
            }

            return bytes;
        }
    }
}
