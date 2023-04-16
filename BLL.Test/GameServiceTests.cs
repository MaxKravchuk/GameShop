using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameShop.BLL.DTO.GameDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services;
using GameShop.BLL.Services.Interfaces;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;
using Moq;
using Xunit;

namespace BLL.Test
{
    public class GameServiceTests : IDisposable
    {
        private readonly Mock<IGameService> MockService;
        private readonly Mock<IUnitOfWork> MockUnitOfWork;
        private readonly GameService gameService;
        private readonly Mock<IMapper> MockMapper;
        private readonly Mock<ILoggerManager> MockLogger;

        private bool _disposed;

        private GameCreateDTO _gameCreateDTO;
        private Game _game;
        private List<Genre> _listOfGenres;
        private List<PlatformType> _listOfPlatformTypes;

        public GameServiceTests()
        {
            MockService = new Mock<IGameService>();
            MockUnitOfWork = new Mock<IUnitOfWork>();
            MockMapper = new Mock<IMapper>();
            MockLogger = new Mock<ILoggerManager>();

            _gameCreateDTO = GetGameCreateDTO();
            _game = GetGame();
            _listOfGenres = GetListOfGenres();
            _listOfPlatformTypes = GetListOfPlatformTypes();

            gameService = new GameService(
                MockUnitOfWork.Object,
                MockMapper.Object,
                MockLogger.Object);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                MockUnitOfWork.Invocations.Clear();
                MockService.Invocations.Clear();
                MockMapper.Invocations.Clear();
                MockLogger.Invocations.Clear();
            }

            _disposed = true;
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

            MockMapper.Setup(m => m.Map<Game>(newGameDTO)).Returns(newGame);

            MockUnitOfWork
                .Setup(u => u.GenreRepository
                .GetAsync(
                It.IsAny<Expression<Func<Genre, bool>>>(),
                It.IsAny<Func<IQueryable<Genre>, IOrderedQueryable<Genre>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(allGenres);

            MockUnitOfWork.Setup(u => u.PlatformTypeRepository
            .GetAsync(
                It.IsAny<Expression<Func<PlatformType, bool>>>(),
                It.IsAny<Func<IQueryable<PlatformType>, IOrderedQueryable<PlatformType>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(allPlatformTypes);

            MockUnitOfWork
                .Setup(u => u.GameRepository.Insert(newGame)).Verifiable();

            // Act
            await gameService.CreateAsync(newGameDTO);

            // Assert
            MockUnitOfWork.Verify(u => u.GameRepository.Insert(newGame), Times.Once);
            MockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            MockLogger.Verify(l => l
            .LogInfo($"Game with key {newGameDTO.Key} created successfully"), Times.Once);
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

            MockMapper.Setup(m => m.Map<Game>(newGameDTO)).Returns(newGame);

            MockUnitOfWork
                .Setup(u => u.GenreRepository
                .GetAsync(
                It.IsAny<Expression<Func<Genre, bool>>>(),
                It.IsAny<Func<IQueryable<Genre>, IOrderedQueryable<Genre>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(allGenres);

            MockUnitOfWork.Setup(u => u.PlatformTypeRepository
            .GetAsync(
                It.IsAny<Expression<Func<PlatformType, bool>>>(),
                It.IsAny<Func<IQueryable<PlatformType>, IOrderedQueryable<PlatformType>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(allPlatformTypes);

            MockUnitOfWork
                .Setup(u => u.GameRepository.Insert(newGame)).Verifiable();

            // Act
            var result = gameService.CreateAsync(newGameDTO);

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

            MockMapper.Setup(m => m.Map<Game>(newGameDTO)).Returns(newGame);

            MockUnitOfWork
                .Setup(u => u.GenreRepository
                .GetAsync(
                It.IsAny<Expression<Func<Genre, bool>>>(),
                It.IsAny<Func<IQueryable<Genre>, IOrderedQueryable<Genre>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(allGenres);

            MockUnitOfWork.Setup(u => u.PlatformTypeRepository
            .GetAsync(
                It.IsAny<Expression<Func<PlatformType, bool>>>(),
                It.IsAny<Func<IQueryable<PlatformType>, IOrderedQueryable<PlatformType>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(allPlatformTypes);

            MockUnitOfWork
                .Setup(u => u.GameRepository.Insert(newGame)).Verifiable();

            // Act
            var result = gameService.CreateAsync(newGameDTO);

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

            MockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(games);

            MockUnitOfWork
                .Setup(u => u.GameRepository.Delete(gameToDelete)).Verifiable();

            // Act
            await gameService.DeleteAsync(gameKey);

            // Assert
            MockUnitOfWork.Verify(u => u.GameRepository.Delete(gameToDelete), Times.Once);
            MockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            MockLogger.Verify(l => l
            .LogInfo($"Game with key {gameToDelete.Key} deleted successfully"), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithWrongGameKey_ShouldThrowNotFoundException()
        {
            // Arrange
            var gameKey = "bad";
            Game gameToDelete = null;
            var games = new List<Game> { gameToDelete };

            MockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(games);

            // Act
            var result = gameService.DeleteAsync(gameKey);

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

            MockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<Game> { _game });

            MockMapper
                .Setup(m => m.Map<GameReadDTO>(_game)).Returns(gameDTO);

            // Act
            var result = await gameService.GetGameByKeyAsync(gameKey);

            // Assert
            MockLogger.Verify(
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

            MockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<Game> { game });

            // Act
            var result = gameService.GetGameByKeyAsync(gameKey);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task GetAllGamesAsync_ShouldReturnFilledGameList()
        {
            // Arrange
            var gameList = new List<Game> { _game };
            var gameListDTO = new List<GameReadListDTO> { new GameReadListDTO() };

            MockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(gameList);

            MockMapper
                .Setup(m => m.Map<IEnumerable<GameReadListDTO>>(gameList)).Returns(gameListDTO);

            // Act
            var result = await gameService.GetAllGamesAsync();

            // Assert
            MockLogger.Verify(
                l => l.LogInfo($"Games successfully returned with array size of {gameListDTO.Count()}"), Times.Once);
            Assert.IsAssignableFrom<IEnumerable<GameReadListDTO>>(result);
            Assert.True(result.Any());
        }

        [Fact]
        public async Task GetAllGamesAsync_ShouldReturnEmptyGameList()
        {
            // Arrange
            var gameList = new List<Game>();
            var gameListDTO = new List<GameReadListDTO>();

            MockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(gameList);

            MockMapper
                .Setup(m => m.Map<IEnumerable<GameReadListDTO>>(gameList)).Returns(gameListDTO);

            // Act
            var result = await gameService.GetAllGamesAsync();

            // Assert
            MockLogger.Verify(
                l => l.LogInfo($"Games successfully returned with array size of {gameListDTO.Count()}"), Times.Once);
            Assert.IsAssignableFrom<IEnumerable<GameReadListDTO>>(result);
            Assert.False(result.Any());
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

            MockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(games.Where(g => g.GameGenres.Any(gg => gg.Id == genreId)));

            MockMapper
                .Setup(m => m.Map<IEnumerable<GameReadListDTO>>(It.IsAny<IEnumerable<Game>>()))
                .Returns(
                    (IEnumerable<Game> source) =>
                    {
                        return source.Select(g => new GameReadListDTO { Id = g.Id });
                    });
            // Act
            var result = await gameService.GetGamesByGenreAsync(genreId);

            // Assert
            Assert.Equal(2, result.Count());
            MockLogger.Verify(l => l.LogInfo(
                $"Games with genreId {genreId} successfully returned with array size of {result.Count()}"), Times.Once);
        }

        [Fact]
        public async Task GetGamesByGenreAsync_ShouldReturnEmptyGameList()
        {
            // Arrange
            var genreId = 1;
            var games = new List<Game>();

            MockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(games);

            // Act
            var result = await gameService.GetGamesByGenreAsync(genreId);

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

            MockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(games.Where(g => g.GamePlatformTypes.Any(gg => gg.Id == platformTypeId)));

            MockMapper
                .Setup(m => m.Map<IEnumerable<GameReadListDTO>>(It.IsAny<IEnumerable<Game>>()))
                .Returns(
                    (IEnumerable<Game> source) =>
                    {
                        return source.Select(g => new GameReadListDTO { Id = g.Id });
                    });
            // Act
            var result = await gameService.GetGamesByGenreAsync(platformTypeId);

            // Assert
            Assert.Equal(2, result.Count());
            MockLogger.Verify(l => l.LogInfo(
                $"Games with genreId {platformTypeId} successfully returned with array size of {result.Count()}"), Times.Once);
        }

        [Fact]
        public async Task GetGamesByPlatformTypeAsync_ShouldReturnEmptyGameList()
        {
            // Arrange
            var platformTypeId = 1;
            var games = new List<Game>();

            MockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(games);

            // Act
            var result = await gameService.GetGamesByGenreAsync(platformTypeId);

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

            MockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<Game> { exGame });

            MockUnitOfWork
                .Setup(u => u.GenreRepository
                .GetAsync(
                It.IsAny<Expression<Func<Genre, bool>>>(),
                It.IsAny<Func<IQueryable<Genre>, IOrderedQueryable<Genre>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(allGenres);

            MockUnitOfWork.Setup(u => u.PlatformTypeRepository
            .GetAsync(
                It.IsAny<Expression<Func<PlatformType, bool>>>(),
                It.IsAny<Func<IQueryable<PlatformType>, IOrderedQueryable<PlatformType>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(allPlatformTypes);

            MockMapper
                .Setup(m => m.Map(gameToUpdate, exGame)).Verifiable();

            // Act
            await gameService.UpdateAsync(gameToUpdate);

            // Assert
            MockUnitOfWork.Verify(u => u.GameRepository.Update(exGame), Times.Once);
            MockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            MockLogger.Verify(l => l
            .LogInfo($"Game with key {gameToUpdate.Key} updated successfully"), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithWrongtModel_ShouldThrowBadRequestException()
        {
            // Arrange
            var gameToUpdate = new GameUpdateDTO() { GenresId = new List<int> { 1 }, PlatformTypeId = new List<int> { 1 } };
            Game exGame = null;

            MockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<Game> { exGame });

            // Act
            var result = gameService.UpdateAsync(gameToUpdate);

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

            MockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<Game> { exGame });

            MockUnitOfWork
                .Setup(u => u.GenreRepository
                .GetAsync(
                It.IsAny<Expression<Func<Genre, bool>>>(),
                It.IsAny<Func<IQueryable<Genre>, IOrderedQueryable<Genre>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(allGenres);

            MockUnitOfWork.Setup(u => u.PlatformTypeRepository
            .GetAsync(
                It.IsAny<Expression<Func<PlatformType, bool>>>(),
                It.IsAny<Func<IQueryable<PlatformType>, IOrderedQueryable<PlatformType>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(allPlatformTypes);

            // Act
            var result = gameService.UpdateAsync(gameToUpdate);

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

            MockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<Game> { exGame });

            MockUnitOfWork
                .Setup(u => u.GenreRepository
                .GetAsync(
                It.IsAny<Expression<Func<Genre, bool>>>(),
                It.IsAny<Func<IQueryable<Genre>, IOrderedQueryable<Genre>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(allGenres);

            MockUnitOfWork.Setup(u => u.PlatformTypeRepository
            .GetAsync(
                It.IsAny<Expression<Func<PlatformType, bool>>>(),
                It.IsAny<Func<IQueryable<PlatformType>, IOrderedQueryable<PlatformType>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(allPlatformTypes);

            // Act
            var result = gameService.UpdateAsync(gameToUpdate);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task GenerateGameFileAsync_WithCorrectKey_ShouldReturnMemoryStreamAndLog()
        {
            // Arrange
            var expectedGame = _game;
            var gameKey = expectedGame.Key;

            MockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<Game> { _game });

            // Act
            var result = await gameService.GenerateGameFileAsync(gameKey);

            // Assert
            MockLogger.Verify(
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

            MockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<Game>());

            // Act + Assert
            await Assert.ThrowsAsync<NotFoundException>(() => gameService.GenerateGameFileAsync(nonExistingGameKey));
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
    }
}
