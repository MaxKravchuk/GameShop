using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using GameShop.BLL.DTO.GenreDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.WebApi.Controllers;
using Moq;
using Xunit;

namespace WebApi.Test.ControllerTests
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
    }
}
