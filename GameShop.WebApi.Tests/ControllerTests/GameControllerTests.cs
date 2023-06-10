using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Results;
using GameShop.BLL.DTO.FilterDTOs;
using GameShop.BLL.DTO.GameDTOs;
using GameShop.BLL.DTO.PaginationDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services.Interfaces;
using GameShop.WebApi.Controllers;
using Moq;
using Xunit;

namespace GameShop.WebApi.Tests.ControllerTests
{
    public class GameControllerTests
    {
        private readonly Mock<IGameService> _mockGameService;
        private readonly GameController _gameController;

        public GameControllerTests()
        {
            _mockGameService = new Mock<IGameService>();
            _gameController = new GameController(_mockGameService.Object);
        }

        [Fact]
        public async Task CreateGameAsync_WithCorrectModel_ShouldReturnStatus200Ok()
        {
            // Assert
            var gameCreateDTO = new GameCreateDTO();

            _mockGameService
                .Setup(s => s
                    .CreateAsync(It.IsAny<GameCreateDTO>())).Verifiable();

            // Act
            var actionResult = await _gameController.CreateGameAsync(gameCreateDTO);

            // Assert
            Assert.IsType<OkResult>(actionResult);
            Assert.NotNull(actionResult);
            _mockGameService.Verify(x => x.CreateAsync(gameCreateDTO), Times.Once);
        }

        [Fact]
        public async Task CreateGameAsync_WithWrongModel_ShouldReturnStatus500NotFound()
        {
            // Assert
            var gameCreateDTO = new GameCreateDTO();

            _mockGameService
                .Setup(s => s
                    .CreateAsync(It.IsAny<GameCreateDTO>())).ThrowsAsync(new NotFoundException());

            // Act
            var actionResult = _gameController.CreateGameAsync(gameCreateDTO);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => actionResult);
        }

        [Fact]
        public async Task UpdateGameAsync_WithCorrectModel_ShouldUpdateAndReturnStatus200Ok()
        {
            // Assert
            var gameUpdateDTO = new GameUpdateDTO();

            _mockGameService
                .Setup(s => s
                    .UpdateAsync(It.IsAny<GameUpdateDTO>())).Verifiable();

            // Act
            var actionResult = await _gameController.UpdateGameAsync(gameUpdateDTO);

            // Assert
            Assert.IsType<OkResult>(actionResult);
            Assert.NotNull(actionResult);
            _mockGameService.Verify(x => x.UpdateAsync(gameUpdateDTO), Times.Once);
        }

        [Fact]
        public async Task UpdateGameAsync_WithWrongModel_ShouldReturnStatus500BadRequest()
        {
            // Assert
            var gameUpdateDTO = new GameUpdateDTO();

            _mockGameService
                .Setup(s => s
                    .UpdateAsync(It.IsAny<GameUpdateDTO>())).ThrowsAsync(new BadRequestException());

            // Act
            var actionResult = _gameController.UpdateGameAsync(gameUpdateDTO);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(() => actionResult);
        }

        [Fact]
        public async Task UpdateGameAsync_WithWrongModel_ShouldReturnStatus500NotFound()
        {
            // Assert
            var gameUpdateDTO = new GameUpdateDTO();

            _mockGameService
                .Setup(s => s
                    .UpdateAsync(It.IsAny<GameUpdateDTO>())).ThrowsAsync(new NotFoundException());

            // Act
            var actionResult = _gameController.UpdateGameAsync(gameUpdateDTO);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => actionResult);
        }

        [Fact]
        public async Task GetGameDetailsByKeyAsync_WithCorrectKey_ReturnGameJsonResult()
        {
            // Assert
            var gameKey = "test";
            var gameReadDTO = new GameReadDTO();

            _mockGameService
                .Setup(s => s
                    .GetGameByKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(gameReadDTO);

            // Act
            var actionResult = await _gameController.GetGameDetailsByKeyAsync(gameKey);

            // Assert
            Assert.IsType<JsonResult<GameReadDTO>>(actionResult);
            Assert.NotNull(actionResult);
            _mockGameService.Verify(s => s.GetGameByKeyAsync(gameKey), Times.Once);
        }

        [Fact]
        public async Task GetGameDetailsByKeyAsync_WithWrongKey_ReturnStatus500NotFound()
        {
            // Assert
            var gameKey = "test";

            _mockGameService
                .Setup(s => s
                    .GetGameByKeyAsync(It.IsAny<string>())).ThrowsAsync(new NotFoundException());

            // Act
            var actionResult = _gameController.GetGameDetailsByKeyAsync(gameKey);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => actionResult);
        }

        [Fact]
        public async Task GetAllGamesAsync_ShouldReturnGameListJsonResult()
        {
            // Arrange
            var gamesList = new List<GameReadListDTO> { new GameReadListDTO() };
            var pagedList = new PagedListViewModel<GameReadListDTO> { Entities = gamesList };
            var gameFilterDTO = new GameFiltersDTO { PageNumber = 1, PageSize = 10 };

            _mockGameService
                .Setup(s => s
                    .GetAllGamesAsync(It.IsAny<GameFiltersDTO>()))
                .ReturnsAsync(pagedList);

            // Act
            var actionResult = await _gameController.GetAllGamesAsync(gameFilterDTO);

            // Assert
            Assert.IsType<JsonResult<PagedListViewModel<GameReadListDTO>>>(actionResult);
            Assert.NotNull(actionResult);
            _mockGameService.Verify(s => s.GetAllGamesAsync(gameFilterDTO), Times.Once);
        }

        [Fact]
        public async Task GetAllGamesByGenre_WithCorrectGenreId_ShouldReturnGameListJsonResult()
        {
            // Arrange
            var genreId = 1;
            var gamesList = new List<GameReadListDTO> { new GameReadListDTO() };

            _mockGameService
                .Setup(s => s
                    .GetGamesByGenreAsync(It.IsAny<int>()))
                .ReturnsAsync(gamesList);

            // Act
            var actionResult = await _gameController.GetAllGamesByGenreAsync(genreId);

            // Assert
            Assert.IsType<JsonResult<IEnumerable<GameReadListDTO>>>(actionResult);
            Assert.NotNull(actionResult);
            _mockGameService.Verify(s => s.GetGamesByGenreAsync(genreId), Times.Once);
        }

        [Fact]
        public async Task GetAllGamesByPlatformType_WithCorrectPlatformTypeId_ShouldReturnGameListJsonResult()
        {
            // Arrange
            var platformTypeId = 1;
            var gamesList = new List<GameReadListDTO> { new GameReadListDTO() };

            _mockGameService
                .Setup(s => s
                    .GetGamesByPlatformTypeAsync(It.IsAny<int>()))
                .ReturnsAsync(gamesList);

            // Act
            var actionResult = await _gameController.GetAllGamesByPlatformTypeAsync(platformTypeId);

            // Assert
            Assert.IsType<JsonResult<IEnumerable<GameReadListDTO>>>(actionResult);
            Assert.NotNull(actionResult);
            _mockGameService.Verify(s => s.GetGamesByPlatformTypeAsync(platformTypeId), Times.Once);
        }

        [Fact]
        public async Task GetAllGamesByPublisherAsync_ReturnsJsonResult_WithGames()
        {
            // Arrange
            int publisherId = 1;
            var expectedGames = new List<GameReadListDTO>
            {
                new GameReadListDTO()
            };

            _mockGameService
                .Setup(s => s
                    .GetGamesByPublisherAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedGames);

            // Act
            var result = await _gameController.GetAllGamesByPublisherAsync(publisherId);

            // Assert
            var jsonResult = Assert.IsType<JsonResult<IEnumerable<GameReadListDTO>>>(result);
            var actualGames = Assert.IsType<List<GameReadListDTO>>(jsonResult.Content);
            Assert.Equal(expectedGames.Count, actualGames.Count);
            _mockGameService.Verify(s => s.GetGamesByPublisherAsync(publisherId), Times.Once);
        }

        [Fact]
        public async Task DeleteGameAsync_WithCorrectGameKey_ShouldDeleteAndReturnStatus200Ok()
        {
            // Arrange
            var gameKey = "test";

            _mockGameService
                .Setup(s => s
                    .DeleteAsync(It.IsAny<string>()))
                .Verifiable();

            // Act
            var actionResult = await _gameController.DeleteGameAsync(gameKey);

            // Assert
            Assert.IsType<OkResult>(actionResult);
            Assert.NotNull(actionResult);
            _mockGameService.Verify(x => x.DeleteAsync(gameKey), Times.Once);
        }

        [Fact]
        public async Task DeleteGameAsync_WithWrongGameKey_ShouldReturnStatus500NotFound()
        {
            // Arrange
            var gameKey = "test";

            _mockGameService
                .Setup(s => s
                    .DeleteAsync(It.IsAny<string>()))
                .ThrowsAsync(new NotFoundException());

            // Act
            var actionResult = _gameController.DeleteGameAsync(gameKey);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => actionResult);
        }

        [Fact]
        public async Task DownloadGame_WithCorrectGameKey_ShouldGenerateFileAndReturnStatus200Ok()
        {
            // Arrange
            var gameKey = "exampleGameKey";
            var fileContent = new MemoryStream();
            var writer = new StreamWriter(fileContent);
            writer.Write("Sample file content");
            writer.Flush();
            fileContent.Position = 0;

            _mockGameService
                .Setup(s => s
                    .GenerateGameFileAsync(It.IsAny<string>()))
                .ReturnsAsync(fileContent);

            // Act
            var actionResult = await _gameController.DownloadGameAsync(gameKey);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<HttpResponseMessage>(actionResult);
            Assert.Equal(HttpStatusCode.OK, actionResult.StatusCode);
            Assert.NotNull(actionResult.Content);
            Assert.IsType<StreamContent>(actionResult.Content);
            var streamContent = Assert.IsType<StreamContent>(actionResult.Content);
            var actualFileContent = await streamContent.ReadAsStringAsync();
            Assert.Equal("Sample file content", actualFileContent);
            Assert.NotNull(actionResult.Content.Headers);
            Assert.Equal(fileContent.Length, actionResult.Content.Headers.ContentLength);
            Assert.Equal("application/octet-stream", actionResult.Content.Headers.ContentType.MediaType);
            Assert.NotNull(actionResult.Content.Headers.ContentDisposition);
            Assert.Equal("attachment", actionResult.Content.Headers.ContentDisposition.DispositionType);
            Assert.Equal($"{gameKey}.bin", actionResult.Content.Headers.ContentDisposition.FileName);
            _mockGameService.Verify(x => x.GenerateGameFileAsync(gameKey), Times.Once);
        }

        [Fact]
        public async Task DownloadGame_WithWrongGameKey_ShouldReturnStatus500NotFound()
        {
            // Arrange
            var gameKey = "exampleGameKey";

            _mockGameService
                .Setup(s => s
                    .GenerateGameFileAsync(It.IsAny<string>()))
                .ThrowsAsync(new NotFoundException());

            // Act
            var actionResult = _gameController.DownloadGameAsync(gameKey);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => actionResult);
        }

        [Fact]
        public async Task GetNumberOfGamesAsync_ReturnsOkResult_WithNumberOfGames()
        {
            // Arrange
            int expectedNumberOfGames = 10;

            _mockGameService
                .Setup(s => s
                    .GetNumberOfGamesAsync())
                .ReturnsAsync(expectedNumberOfGames);

            // Act
            var result = await _gameController.GetNumberOfGamesAsync();

            // Assert
            var okResult = Assert.IsType<OkNegotiatedContentResult<int>>(result);
            var actualNumberOfGames = Assert.IsType<int>(okResult.Content);
            Assert.Equal(expectedNumberOfGames, actualNumberOfGames);
            _mockGameService.Verify(s => s.GetNumberOfGamesAsync(), Times.Once);
        }
    }
}
