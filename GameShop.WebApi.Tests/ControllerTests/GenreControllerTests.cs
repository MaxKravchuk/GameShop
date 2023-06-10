using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using GameShop.BLL.DTO.GenreDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.WebApi.Controllers;
using Moq;
using Xunit;

namespace GameShop.WebApi.Tests.ControllerTests
{
    public class GenreControllerTests
    {
        private readonly Mock<IGenreService> _mockGenreService;
        private readonly GenreController _genreController;

        public GenreControllerTests()
        {
            _mockGenreService = new Mock<IGenreService>();
            _genreController = new GenreController(_mockGenreService.Object);
        }

        [Fact]
        public async Task GetAllGenres_ShouldReturnListOfGenres()
        {
            // Arrange
            var genreList = new List<GenreReadListDTO> { new GenreReadListDTO() };

            _mockGenreService
                .Setup(s => s
                    .GetAsync())
                .ReturnsAsync(genreList);

            // Act
            var actionResult = await _genreController.GetAllGenresAsync();

            // Assert
            Assert.IsType<JsonResult<IEnumerable<GenreReadListDTO>>>(actionResult);
            Assert.NotNull(actionResult);
            _mockGenreService.Verify(s => s.GetAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllGenres_ShouldReturnEmptyListOfGenres()
        {
            // Arrange
            var genreList = new List<GenreReadListDTO>();

            _mockGenreService
                .Setup(s => s
                    .GetAsync())
                .ReturnsAsync(genreList);

            // Act
            var actionResult = await _genreController.GetAllGenresAsync();

            // Assert
            Assert.IsType<JsonResult<IEnumerable<GenreReadListDTO>>>(actionResult);
            Assert.NotNull(actionResult);
            _mockGenreService.Verify(s => s.GetAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateGenreAsync_WithValidGenre_ReturnsOkResult()
        {
            // Arrange
            var genreCreateDTO = new GenreCreateDTO { Name = "Action" };
            _mockGenreService
                .Setup(s => s
                    .CreateAsync(It.IsAny<GenreCreateDTO>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _genreController.CreateGenreAsync(genreCreateDTO);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockGenreService.Verify(s => s.CreateAsync(genreCreateDTO), Times.Once);
        }

        [Fact]
        public async Task UpdateGenreAsync_WithValidGenre_ReturnsOkResult()
        {
            // Arrange
            var genreUpdateDTO = new GenreUpdateDTO { Id = 1, Name = "Adventure" };

            _mockGenreService
                .Setup(s => s
                    .UpdateAsync(It.IsAny<GenreUpdateDTO>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _genreController.UpdateGenreAsync(genreUpdateDTO);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockGenreService.Verify(s => s.UpdateAsync(genreUpdateDTO), Times.Once);
        }

        [Fact]
        public async Task DeleteGenreAsync_WithValidGenreId_ReturnsOkResult()
        {
            // Arrange
            int genreId = 1;

            _mockGenreService
                .Setup(s => s.
                    DeleteAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _genreController.DeleteGenreAsync(genreId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockGenreService.Verify(s => s.DeleteAsync(genreId), Times.Once);
        }
    }
}
