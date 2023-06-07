using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using GameShop.BLL.DTO.GenreDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;
using Moq;
using Xunit;

namespace GameShop.BLL.Tests.ServiceTests
{
    public class GenreServiceTests : IDisposable
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly GenreService _genreService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerManager> _mockLogger;
        private readonly Mock<IValidator<GenreCreateDTO>> _mockValidator;

        private bool _disposed;

        public GenreServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerManager>();
            _mockValidator = new Mock<IValidator<GenreCreateDTO>>();

            _genreService = new GenreService(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockLogger.Object,
                _mockValidator.Object);
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

            _mockMapper
                .Setup(m => m.Map<Genre>(genreToAddDTO)).Returns(genreToAdd);

            _mockUnitOfWork
                .Setup(u => u.GenreRepository
                    .Insert(genreToAdd)).Verifiable();

            // Act
            await _genreService.CreateAsync(genreToAddDTO);

            // Assert
            _mockUnitOfWork.Verify(u => u.GenreRepository.Insert(genreToAdd), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            _mockLogger.Verify(
                l => l.LogInfo($"Genre with name {genreToAdd.Name} was created successfully"), Times.Once);
        }

        [Fact]
        public async Task DeleteGenreAsync_WithCorrectId_ShouldDeleteAndLog()
        {
            // Arrange
            var id = 1;
            var genreToDelete = new Genre { Id = 1 };

            _mockUnitOfWork
                .Setup(u => u.GenreRepository
                    .GetByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(genreToDelete);

            _mockUnitOfWork
                .Setup(u => u.GenreRepository
                    .Delete(genreToDelete))
                .Verifiable();

            // Act
            await _genreService.DeleteAsync(id);

            // Assert
            _mockUnitOfWork.Verify(u => u.GenreRepository.Delete(genreToDelete), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            _mockLogger.Verify(
                l => l.LogInfo($"Genre with id {id} was deleted successfully"), Times.Once);
        }

        [Fact]
        public async Task DeleteGenreAsync_WithWrongId_ShouldThrowNotFoundException()
        {
            // Arrange
            var id = -1;
            Genre genreToDelete = null;

            _mockUnitOfWork
                .Setup(u => u.GenreRepository
                    .GetByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(genreToDelete);

            // Act
            var result = _genreService.DeleteAsync(id);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task GetGenresAsync_ShouldReturnListOfGenres()
        {
            // Arrange
            var genreList = new List<Genre> { new Genre() };
            var genreListDTO = new List<GenreReadListDTO> { new GenreReadListDTO() };

            _mockUnitOfWork
                .Setup(u => u.GenreRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Genre, bool>>>(),
                        It.IsAny<Func<IQueryable<Genre>, IOrderedQueryable<Genre>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(genreList);

            _mockMapper
                .Setup(m => m.Map<IEnumerable<GenreReadListDTO>>(genreList)).Returns(genreListDTO);

            // Act
            var result = await _genreService.GetAsync();

            // Assert
            _mockLogger.Verify(
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

            _mockUnitOfWork
                .Setup(u => u.GenreRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Genre, bool>>>(),
                        It.IsAny<Func<IQueryable<Genre>, IOrderedQueryable<Genre>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(genreList);

            _mockMapper
                .Setup(m => m.Map<IEnumerable<GenreReadListDTO>>(genreList)).Returns(genreListDTO);

            // Act
            var result = await _genreService.GetAsync();

            // Assert
            _mockLogger.Verify(
                l => l.LogInfo($"Genres were returned successfully in array size of {genreListDTO.Count()}"), Times.Once);
            Assert.IsAssignableFrom<IEnumerable<GenreReadListDTO>>(result);
            Assert.False(result.Any());
        }

        [Fact]
        public async Task UpdateGenreAsync_WithCorrectModel_ShouldUpdateAndLog()
        {
            // Arrange
            var genreToUpdate = new Genre { Id = 1 };
            var genreToUpdateDTO = new GenreUpdateDTO { Id = 1 };

            _mockUnitOfWork
                .Setup(u => u.GenreRepository
                    .GetByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(genreToUpdate);

            _mockMapper
                .Setup(m => m.Map(genreToUpdateDTO, genreToUpdate)).Verifiable();

            // Act
            await _genreService.UpdateAsync(genreToUpdateDTO);

            // Assert
            _mockUnitOfWork.Verify(u => u.GenreRepository.Update(genreToUpdate), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            _mockLogger.Verify(
                l => l.LogInfo($"Genre with id {genreToUpdateDTO.Id} was updated successfully"), Times.Once);
        }

        [Fact]
        public async Task UpdateGenreAsync_WithWrongModel_ShouldThrowNotFoundException()
        {
            // Arrange
            Genre genreToUpdate = null;
            var genreToUpdateDTO = new GenreUpdateDTO { Id = -1 };

            _mockUnitOfWork
                .Setup(u => u.GenreRepository
                    .GetByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(genreToUpdate);

            // Act
            var result = _genreService.UpdateAsync(genreToUpdateDTO);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
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
            }

            _disposed = true;
        }
    }
}
