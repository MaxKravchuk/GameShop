using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using GameShop.BLL.DTO.FilterDTOs;
using GameShop.BLL.DTO.GameDTOs;
using GameShop.BLL.DTO.PaginationDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Filters.Interfaces;
using GameShop.BLL.Pagination;
using GameShop.BLL.Services;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.BLL.Strategies.Interfaces.Factories;
using GameShop.BLL.Strategies.Interfaces.Strategies;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;
using Moq;
using Xunit;

namespace GameShop.BLL.Tests.ServiceTests
{
    public class GameServiceTests : IDisposable
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly GameService _gameService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerManager> _mockLogger;
        private readonly Mock<IValidator<GameCreateDTO>> _mockValidator;
        private readonly Mock<IFiltersFactory<IEnumerable<Game>>> _mockFiltersFactory;
        private readonly Mock<IGameSortingFactory> _mockGameSortingFactory;

        private bool _disposed;

        private GameCreateDTO _gameCreateDTO;
        private Game _game;
        private GameFiltersDTO _gameFiltersDTO;
        private List<Genre> _listOfGenres;
        private List<PlatformType> _listOfPlatformTypes;

        public GameServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerManager>();
            _mockValidator = new Mock<IValidator<GameCreateDTO>>();
            _mockFiltersFactory = new Mock<IFiltersFactory<IEnumerable<Game>>>();
            _mockGameSortingFactory = new Mock<IGameSortingFactory>();

            _gameCreateDTO = GetGameCreateDTO();
            _game = GetGame();
            _listOfGenres = GetListOfGenres();
            _listOfPlatformTypes = GetListOfPlatformTypes();

            _gameService = new GameService(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockLogger.Object,
                _mockValidator.Object,
                _mockFiltersFactory.Object,
                _mockGameSortingFactory.Object);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task CreateGame_ShouldCreateGameAndLogInfo()
        {
            // Arrange
            var newGameDTO = _gameCreateDTO;
            var newGame = _game;
            var allGenres = _listOfGenres;
            var allPlatformTypes = _listOfPlatformTypes;

            _mockMapper.Setup(m => m.Map<Game>(newGameDTO)).Returns(newGame);

            _mockUnitOfWork
                .Setup(u => u.GenreRepository
                .GetAsync(
                It.IsAny<Expression<Func<Genre, bool>>>(),
                It.IsAny<Func<IQueryable<Genre>, IOrderedQueryable<Genre>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(allGenres);

            _mockUnitOfWork.Setup(u => u.PlatformTypeRepository
            .GetAsync(
                It.IsAny<Expression<Func<PlatformType, bool>>>(),
                It.IsAny<Func<IQueryable<PlatformType>, IOrderedQueryable<PlatformType>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(allPlatformTypes);

            _mockUnitOfWork
                .Setup(u => u.GameRepository.Insert(newGame)).Verifiable();

            // Act
            await _gameService.CreateAsync(newGameDTO);

            // Assert
            _mockUnitOfWork.Verify(u => u.GameRepository.Insert(newGame), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            _mockLogger.Verify(
                l => l.LogInfo($"Game with key {newGameDTO.Key} created successfully"), Times.Once);
        }

        [Fact]
        public async Task CreateGame_ShouldThrowNotFoundExceptionForGenres()
        {
            // Arrange
            var newGameDTO = _gameCreateDTO;
            newGameDTO.GenresId = new List<int> { 0 };
            var newGame = _game;
            var allGenres = _listOfGenres;
            var allPlatformTypes = _listOfPlatformTypes;

            _mockMapper.Setup(m => m.Map<Game>(newGameDTO)).Returns(newGame);

            _mockUnitOfWork
                .Setup(u => u.GenreRepository
                .GetAsync(
                It.IsAny<Expression<Func<Genre, bool>>>(),
                It.IsAny<Func<IQueryable<Genre>, IOrderedQueryable<Genre>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(allGenres);

            _mockUnitOfWork.Setup(u => u.PlatformTypeRepository
            .GetAsync(
                It.IsAny<Expression<Func<PlatformType, bool>>>(),
                It.IsAny<Func<IQueryable<PlatformType>, IOrderedQueryable<PlatformType>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(allPlatformTypes);

            _mockUnitOfWork
                .Setup(u => u.GameRepository.Insert(newGame)).Verifiable();

            // Act
            var result = _gameService.CreateAsync(newGameDTO);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task CreateGame_ShouldThrowNotFoundExceptionForPlatformTypes()
        {
            // Arrange
            var newGameDTO = _gameCreateDTO;
            newGameDTO.PlatformTypeId = new List<int> { 0 };
            var newGame = _game;
            var allGenres = _listOfGenres;
            var allPlatformTypes = _listOfPlatformTypes;

            _mockMapper.Setup(m => m.Map<Game>(newGameDTO)).Returns(newGame);

            _mockUnitOfWork
                .Setup(u => u.GenreRepository
                .GetAsync(
                It.IsAny<Expression<Func<Genre, bool>>>(),
                It.IsAny<Func<IQueryable<Genre>, IOrderedQueryable<Genre>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(allGenres);

            _mockUnitOfWork.Setup(u => u.PlatformTypeRepository
            .GetAsync(
                It.IsAny<Expression<Func<PlatformType, bool>>>(),
                It.IsAny<Func<IQueryable<PlatformType>, IOrderedQueryable<PlatformType>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(allPlatformTypes);

            _mockUnitOfWork
                .Setup(u => u.GameRepository.Insert(newGame)).Verifiable();

            // Act
            var result = _gameService.CreateAsync(newGameDTO);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task DeleteAsync__WithCorrectGameKey_ShouldDeleteGameAndLogInfo()
        {
            // Arrange
            var gameToDelete = _game;
            var gameKey = _game.Key;
            var games = new List<Game> { gameToDelete };

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(games);

            _mockUnitOfWork
                .Setup(u => u.GameRepository.Delete(gameToDelete)).Verifiable();

            // Act
            await _gameService.DeleteAsync(gameKey);

            // Assert
            _mockUnitOfWork.Verify(u => u.GameRepository.Delete(gameToDelete), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            _mockLogger.Verify(
                l => l.LogInfo($"Game with key {gameToDelete.Key} deleted successfully"), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithWrongGameKey_ShouldThrowNotFoundException()
        {
            // Arrange
            var gameKey = "bad";
            Game gameToDelete = null;
            var games = new List<Game> { gameToDelete };

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(games);

            // Act
            var result = _gameService.DeleteAsync(gameKey);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task GetGameByKeyAsync_WithCorrectGameKey_ShouldReturnGame()
        {
            // Arrange
            var expectedGame = _game;
            var gameKey = _game.Key;
            var gameDTO = new GameReadDTO();

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<Game> { _game });

            _mockMapper
                .Setup(m => m.Map<GameReadDTO>(_game)).Returns(gameDTO);

            // Act
            var result = await _gameService.GetGameByKeyAsync(gameKey);

            // Assert
            _mockLogger.Verify(
                l => l.LogInfo($"Game with key {gameKey} returned successfully"), Times.Once);
            Assert.IsType<GameReadDTO>(result);
            Assert.Equal(gameDTO, result);
        }

        [Fact]
        public async Task GetGameByKeyAsync_WithWrongGameKey_ShouldThrowNotFoundException()
        {
            // Arrange
            var gameKey = "test";
            Game game = null;

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<Game> { game });

            // Act
            var result = _gameService.GetGameByKeyAsync(gameKey);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task GetAllGamesAsync_WithCorrectSortingOption_ShouldReturnFilledSortedGameList()
        {
            // Arrange
            var gameList = new List<Game> { _game };
            var gameListDTO = new List<GameReadListDTO> { new GameReadListDTO() };
            var pagedGameList = new PagedListViewModel<GameReadListDTO> { Entities = gameListDTO };
            var gameFiltersDTO = new GameFiltersDTO { PageNumber = 1, PageSize = 10, SortingOption = "AscPrice" };

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(gameList);

            _mockMapper
                .Setup(m => m.Map<PagedListViewModel<GameReadListDTO>>(gameList))
                .Returns(pagedGameList);

            // Act
            var result = await _gameService.GetAllGamesAsync(gameFiltersDTO);

            // Assert
            _mockLogger.Verify(
                l => l.LogInfo($"Games successfully returned with array size of {gameListDTO.Count()}"), Times.Once);
            Assert.IsAssignableFrom<IEnumerable<GameReadListDTO>>(result);

            Assert.True(result.Entities.Any());
        }

        [Fact]
        public async Task GetAllGamesAsync_WithCorrectSortingOption_ShouldReturnEmptyGameList()
        {
            // Arrange
            var gameList = new List<Game>();
            var gameListDTO = new List<GameReadListDTO>();
            var pagedGameList = new PagedListViewModel<GameReadListDTO> { Entities = gameListDTO };
            var gameFiltersDTO = new GameFiltersDTO { PageNumber = 1, PageSize = 10, SortingOption = "AscPrice" };

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(gameList);

            _mockMapper
                .Setup(m => m.Map<IEnumerable<GameReadListDTO>>(gameList)).Returns(gameListDTO);

            // Act
            var result = await _gameService.GetAllGamesAsync(gameFiltersDTO);

            // Assert
            _mockLogger.Verify(
                l => l.LogInfo($"Games successfully returned with array size of {gameListDTO.Count()}"), Times.Once);
            Assert.IsAssignableFrom<IEnumerable<GameReadListDTO>>(result);
            Assert.False(result.Entities.Any());
        }

        [Fact]
        public async Task GetAllGamesAsync_WithEmptySortingOption_ShouldReturnGameList()
        {
            // Arrange
            var gameList = new List<Game> { _game };
            var gameListDTO = new List<GameReadListDTO> { new GameReadListDTO() };
            var pagedGameList = new PagedListViewModel<GameReadListDTO> { Entities = gameListDTO };
            var gameFiltersDTO = new GameFiltersDTO { PageNumber = 1, PageSize = 10, SortingOption = "AscPrice" };

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(gameList);

            _mockMapper
                .Setup(m => m.Map<PagedListViewModel<GameReadListDTO>>(gameList))
                .Returns(pagedGameList);

            // Act
            var result = await _gameService.GetAllGamesAsync(gameFiltersDTO);

            // Assert
            _mockLogger.Verify(
                l => l.LogInfo($"Games successfully returned with array size of {gameListDTO.Count()}"), Times.Once);
            Assert.IsAssignableFrom<IEnumerable<GameReadListDTO>>(result);

            Assert.True(result.Entities.Any());
        }

        [Fact]
        public async Task GetAllGamesAsync_WithWrongSortingOption_ShouldThrowBadRequestException()
        {
            // Arrange
            var gameList = new List<Game> { _game };
            var gameFiltersDTO = new GameFiltersDTO { PageNumber = 1, PageSize = 10, SortingOption = "wrong" };

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(gameList);

            // Act
            var result = _gameService.GetAllGamesAsync(gameFiltersDTO);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(() => result);
        }

        [Fact]
        public async Task GetGamesByGenreAsync_ShouldReturnFilledGameList()
        {
            // Arrange
            var genreId = 1;
            var games = new List<Game>
            {
                new Game { Id = 1, GameGenres = new List<Genre> { new Genre { Id = genreId } } },
                new Game { Id = 2, GameGenres = new List<Genre> { new Genre { Id = genreId } } },
                new Game { Id = 3, GameGenres = new List<Genre> { new Genre { Id = 2 } } },
            };

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(games.Where(g => g.GameGenres.Any(gg => gg.Id == genreId)));

            _mockMapper
                .Setup(m => m.Map<IEnumerable<GameReadListDTO>>(It.IsAny<IEnumerable<Game>>()))
                .Returns(
                    (IEnumerable<Game> source) =>
                    {
                        return source.Select(g => new GameReadListDTO { Id = g.Id });
                    });

            // Act
            var result = await _gameService.GetGamesByGenreAsync(genreId);

            // Assert
            Assert.Equal(2, result.Count());
            _mockLogger.Verify(
                l => l.LogInfo(
                    $"Games with genreId {genreId} successfully returned with array size of {result.Count()}"), Times.Once);
        }

        [Fact]
        public async Task GetGamesByGenreAsync_ShouldReturnEmptyGameList()
        {
            // Arrange
            var genreId = 1;
            var games = new List<Game>();

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(games);

            // Act
            var result = await _gameService.GetGamesByGenreAsync(genreId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetGamesByPlatformTypeAsync_ShouldReturnFilledGameList()
        {
            // Arrange
            var platformTypeId = 1;
            var games = new List<Game>
            {
                new Game { Id = 1, GamePlatformTypes = new List<PlatformType> { new PlatformType { Id = platformTypeId } } },
                new Game { Id = 2, GamePlatformTypes = new List<PlatformType> { new PlatformType { Id = platformTypeId } } },
                new Game { Id = 3, GamePlatformTypes = new List<PlatformType> { new PlatformType { Id = 2 } } },
            };

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(games.Where(g => g.GamePlatformTypes.Any(gg => gg.Id == platformTypeId)));

            _mockMapper
                .Setup(m => m.Map<IEnumerable<GameReadListDTO>>(It.IsAny<IEnumerable<Game>>()))
                .Returns(
                    (IEnumerable<Game> source) =>
                    {
                        return source.Select(g => new GameReadListDTO { Id = g.Id });
                    });

            // Act
            var result = await _gameService.GetGamesByGenreAsync(platformTypeId);

            // Assert
            Assert.Equal(2, result.Count());
            _mockLogger.Verify(
                l => l.LogInfo(
                $"Games with genreId {platformTypeId} successfully returned with array size of {result.Count()}"), Times.Once);
        }

        [Fact]
        public async Task GetGamesByPlatformTypeAsync_ShouldReturnEmptyGameList()
        {
            // Arrange
            var platformTypeId = 1;
            var games = new List<Game>();

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(games);

            // Act
            var result = await _gameService.GetGamesByGenreAsync(platformTypeId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task UpdateAsync_WithCorrectModel_ShouldUpdateAndLog()
        {
            // Arrange
            var gameToUpdate = new GameUpdateDTO() { GenresId = new List<int> { 1 }, PlatformTypeId = new List<int> { 1 } };
            var exGame = _game;
            var allGenres = _listOfGenres;
            var allPlatformTypes = _listOfPlatformTypes;

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<Game> { exGame });

            MockSetupForGenres(allGenres);
            MockSetupForPlatformTypes(allPlatformTypes);

            _mockMapper
                .Setup(m => m.Map(gameToUpdate, exGame)).Verifiable();

            // Act
            await _gameService.UpdateAsync(gameToUpdate);

            // Assert
            _mockUnitOfWork.Verify(u => u.GameRepository.Update(exGame), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            _mockLogger.Verify(
                l => l.LogInfo($"Game with key {gameToUpdate.Key} updated successfully"), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithWrongtModel_ShouldThrowBadRequestException()
        {
            // Arrange
            var gameToUpdate = new GameUpdateDTO() { GenresId = new List<int> { 1 }, PlatformTypeId = new List<int> { 1 } };
            Game exGame = null;

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<Game> { exGame });

            // Act
            var result = _gameService.UpdateAsync(gameToUpdate);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(() => result);
        }

        [Fact]
        public async Task UpdateAsync_WithCorrectModel_ShouldThrowNotFoundExceptionForGenres()
        {
            // Arrange
            var gameToUpdate = new GameUpdateDTO() { GenresId = new List<int> { 0 }, PlatformTypeId = new List<int> { 1 } };
            var exGame = _game;
            var allGenres = _listOfGenres;
            var allPlatformTypes = _listOfPlatformTypes;

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<Game> { exGame });

            MockSetupForGenres(allGenres);
            MockSetupForPlatformTypes(allPlatformTypes);

            // Act
            var result = _gameService.UpdateAsync(gameToUpdate);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task UpdateAsync_WithCorrectModel_ShouldThrowNotFoundExceptionForPlatformTypes()
        {
            // Arrange
            var gameToUpdate = new GameUpdateDTO() { GenresId = new List<int> { 1 }, PlatformTypeId = new List<int> { 0 } };
            var exGame = _game;
            var allGenres = _listOfGenres;
            var allPlatformTypes = _listOfPlatformTypes;

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<Game> { exGame });

            MockSetupForGenres(allGenres);
            MockSetupForPlatformTypes(allPlatformTypes);

            // Act
            var result = _gameService.UpdateAsync(gameToUpdate);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task GenerateGameFileAsync_WithCorrectKey_ShouldReturnMemoryStreamAndLog()
        {
            // Arrange
            var expectedGame = _game;
            var gameKey = expectedGame.Key;

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<Game> { _game });

            // Act
            var result = await _gameService.GenerateGameFileAsync(gameKey);

            // Assert
            _mockLogger.Verify(
                l => l.LogInfo($"Upload data successfully created for game with key {gameKey}"), Times.Once);
            var bytes = result.ToArray();
            var dataToDownload = $"Game-{_game.Name}|{_game.Key}|{_game.Description}";
            var expectedBytes = Encoding.ASCII.GetBytes(dataToDownload);
            Assert.Equal(expectedBytes, bytes);
        }

        [Fact]
        public async Task GenerateGameFileAsync_WithWrongKey_ShouldThrowNotFoundError()
        {
            // Arrange
            string nonExistingGameKey = "NonExistingKey";

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<Game>());

            // Act + Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _gameService.GenerateGameFileAsync(nonExistingGameKey));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _mockUnitOfWork.Invocations.Clear();
                _mockMapper.Invocations.Clear();
                _mockLogger.Invocations.Clear();
                _mockValidator.Invocations.Clear();
            }

            _disposed = true;
        }

        private GameCreateDTO GetGameCreateDTO()
        {
            return new GameCreateDTO
            {
                Key = "new_game_key",
                GenresId = new List<int> { 1 },
                PlatformTypeId = new List<int> { 1 }
            };
        }

        private Game GetGame()
        {
            return new Game()
            {
                Key = "new_game_key",
            };
        }

        private List<Genre> GetListOfGenres()
        {
            return new List<Genre>
            {
                new Genre { Id = 1 },
            };
        }

        private List<PlatformType> GetListOfPlatformTypes()
        {
            return new List<PlatformType>
            {
                new PlatformType { Id = 1 },
            };
        }

        private void MockSetupForGenres(List<Genre> allGenres)
        {
            _mockUnitOfWork
                .Setup(u => u.GenreRepository
                .GetAsync(
                It.IsAny<Expression<Func<Genre, bool>>>(),
                It.IsAny<Func<IQueryable<Genre>, IOrderedQueryable<Genre>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(allGenres);
        }

        private void MockSetupForPlatformTypes(List<PlatformType> allPlatformTypes)
        {
            _mockUnitOfWork.Setup(u => u.PlatformTypeRepository
            .GetAsync(
                It.IsAny<Expression<Func<PlatformType, bool>>>(),
                It.IsAny<Func<IQueryable<PlatformType>, IOrderedQueryable<PlatformType>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(allPlatformTypes);
        }
    }
}
