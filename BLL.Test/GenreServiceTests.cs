using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using GameShop.BLL.DTO.GenreDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services;
using GameShop.BLL.Services.Interfaces;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;
using Moq;
using Xunit;

namespace BLL.Test
{
    public class GenreServiceTests : IDisposable
    {
        private readonly Mock<IGenreService> MockService;
        private readonly Mock<IUnitOfWork> MockUnitOfWork;
        private readonly GenreService GenreService;
        private readonly Mock<IMapper> MockMapper;
        private readonly Mock<ILoggerManager> MockLogger;

        private bool _disposed;

        public GenreServiceTests()
        {
            MockService = new Mock<IGenreService>();
            MockUnitOfWork = new Mock<IUnitOfWork>();
            MockMapper = new Mock<IMapper>();
            MockLogger = new Mock<ILoggerManager>();

            GenreService = new GenreService(
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
        public async Task CreateGenreAsync_WithCorrectModel_ShouldCreateAndLog()
        {
            // Arrange
            var genreToAddDTO = new GenreCreateDTO();
            var genreToAdd = new Genre();

            MockMapper
                .Setup(m => m.Map<Genre>(genreToAddDTO)).Returns(genreToAdd);

            MockUnitOfWork
                .Setup(u => u.GenreRepository
                    .Insert(genreToAdd)).Verifiable();

            // Act
            await GenreService.CreateAsync(genreToAddDTO);

            // Assert
            MockUnitOfWork.Verify(u => u.GenreRepository.Insert(genreToAdd), Times.Once);
            MockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            MockLogger.Verify(l => l
            .LogInfo($"Genre with name {genreToAdd.Name} was created successfully"), Times.Once);
        }

        [Fact]
        public async Task DeleteGenreAsync_WithCorrectId_ShouldDeleteAndLog()
        {
            // Arrange
            var id = 1;
            var genreToDelete = new Genre { Id = 1 };

            MockUnitOfWork
                .Setup(u => u.GenreRepository
                    .GetByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(genreToDelete);

            MockUnitOfWork
                .Setup(u => u.GenreRepository
                    .Delete(genreToDelete))
                .Verifiable();

            // Act
            await GenreService.DeleteAsync(id);

            // Assert
            MockUnitOfWork.Verify(u => u.GenreRepository.Delete(genreToDelete), Times.Once);
            MockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            MockLogger.Verify(l => l
            .LogInfo($"Genre with id {id} was deleted successfully"), Times.Once);
        }

        [Fact]
        public async Task DeleteGenreAsync_WithWrongId_ShouldThrowNotFoundException()
        {
            // Arrange
            var id = -1;
            Genre genreToDelete = null;

            MockUnitOfWork
                .Setup(u => u.GenreRepository
                    .GetByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(genreToDelete);

            // Act
            var result = GenreService.DeleteAsync(id);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task GetGenresAsync_ShouldReturnListOfGenres()
        {
            // Arrange
            var genreList = new List<Genre> { new Genre() };
            var genreListDTO = new List<GenreReadListDTO> { new GenreReadListDTO() };

            MockUnitOfWork
                .Setup(u => u.GenreRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Genre, bool>>>(),
                        It.IsAny<Func<IQueryable<Genre>, IOrderedQueryable<Genre>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(genreList);

            MockMapper
                .Setup(m => m.Map<IEnumerable<GenreReadListDTO>>(genreList)).Returns(genreListDTO);

            // Act
            var result = await GenreService.GetAsync();

            // Assert
            MockLogger.Verify(
                l => l.LogInfo($"Genres were returned successfully in array size of {genreListDTO.Count()}"), Times.Once);
            Assert.IsAssignableFrom<IEnumerable<GenreReadListDTO>>(result);
            Assert.True(result.Any());
        }

        [Fact]
        public async Task GetGenresAsync_ShouldReturnEmptyListOfGenres()
        {
            // Arrange
            var genreList = new List<Genre>();
            var genreListDTO = new List<GenreReadListDTO>();

            MockUnitOfWork
                .Setup(u => u.GenreRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Genre, bool>>>(),
                        It.IsAny<Func<IQueryable<Genre>, IOrderedQueryable<Genre>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(genreList);

            MockMapper
                .Setup(m => m.Map<IEnumerable<GenreReadListDTO>>(genreList)).Returns(genreListDTO);

            // Act
            var result = await GenreService.GetAsync();

            // Assert
            MockLogger.Verify(
                l => l.LogInfo($"Genres were returned successfully in array size of {genreListDTO.Count()}"), Times.Once);
            Assert.IsAssignableFrom<IEnumerable<GenreReadListDTO>>(result);
            Assert.False(result.Any());
        }

        [Fact]
        public async Task GetGenreById_WithCorrectId_ShouldReturnGenreAndLog()
        {
            // Arrange
            var id = 1;
            var genre = new Genre { Id = id };
            var genreDTO = new GenreReadDTO { Id = id };

            MockUnitOfWork
                .Setup(u => u.GenreRepository
                    .GetByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(genre);

            MockMapper
                .Setup(m => m.Map<GenreReadDTO>(genre)).Returns(genreDTO);

            // Act
            var result = await GenreService.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<GenreReadDTO>(result);
            MockLogger.Verify(l => l.LogInfo(
                $"Genre with id {id} successfully returned"), Times.Once);
        }

        [Fact]
        public async Task GetGenreById_WithWrongId_ShouldThrowNotFoundAsync()
        {
            // Arrange
            var id = 0;
            Genre genre = null;

            MockUnitOfWork
                .Setup(u => u.GenreRepository
                    .GetByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(genre);

            // Act
            var result = GenreService.GetByIdAsync(id);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task UpdateGenreAsync_WithCorrectModel_ShouldUpdateAndLog()
        {
            // Arrange
            var genreToUpdate = new Genre { Id = 1 };
            var genreToUpdateDTO = new GenreUpdateDTO { Id = 1 };

            MockUnitOfWork
                .Setup(u => u.GenreRepository
                    .GetByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(genreToUpdate);

            MockMapper
                .Setup(m => m.Map(genreToUpdateDTO, genreToUpdate)).Verifiable();

            // Act
            await GenreService.UpdateAsync(genreToUpdateDTO);

            // Assert
            MockUnitOfWork.Verify(u => u.GenreRepository.Update(genreToUpdate), Times.Once);
            MockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            MockLogger.Verify(l => l
                .LogInfo($"Genre with id {genreToUpdateDTO.Id} was updated successfully"), Times.Once);
        }

        [Fact]
        public async Task UpdateGenreAsync_WithWrongModel_ShouldThrowNotFoundException()
        {
            // Arrange
            Genre genreToUpdate = null; 
            var genreToUpdateDTO = new GenreUpdateDTO { Id = -1 };

            MockUnitOfWork
                .Setup(u => u.GenreRepository
                    .GetByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(genreToUpdate);

            // Act
            var result = GenreService.UpdateAsync(genreToUpdateDTO);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }
    }
}
