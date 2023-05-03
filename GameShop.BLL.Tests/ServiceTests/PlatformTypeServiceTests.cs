using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using GameShop.BLL.DTO.PlatformTypeDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;
using Moq;
using Xunit;

namespace GameShop.BLL.Tests.ServiceTests
{
    public class PlatformTypeServiceTests : IDisposable
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly PlatformTypeService _platformTypeService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerManager> _mockLogger;

        private bool _disposed;

        public PlatformTypeServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerManager>();

            _platformTypeService = new PlatformTypeService(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockLogger.Object);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task CreatePlatformTypeAsync_WithCorrectModel_ShouldCreateAndLog()
        {
            // Arrange
            var platformTypeToAddDTO = new PlatformTypeCreateDTO();
            var platformToAdd = new PlatformType();

            _mockMapper
                .Setup(m => m.Map<PlatformType>(platformTypeToAddDTO)).Returns(platformToAdd);

            _mockUnitOfWork
                .Setup(u => u.PlatformTypeRepository
                    .Insert(platformToAdd)).Verifiable();

            // Act
            await _platformTypeService.CreateAsync(platformTypeToAddDTO);

            // Assert
            _mockUnitOfWork.Verify(u => u.PlatformTypeRepository.Insert(platformToAdd), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            _mockLogger.Verify(
                l => l.LogInfo($"Platform type with type {platformTypeToAddDTO.Type} was created successfully"), Times.Once);
        }

        [Fact]
        public async Task DeletePlatformTypeAsync_WithCorrectId_ShouldDeleteAndLog()
        {
            // Arrange
            var id = 1;
            var platformTypeToDelete = new PlatformType { Id = 1 };

            _mockUnitOfWork
                .Setup(u => u.PlatformTypeRepository
                    .GetByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(platformTypeToDelete);

            _mockUnitOfWork
                .Setup(u => u.PlatformTypeRepository
                    .Delete(platformTypeToDelete))
                .Verifiable();

            // Act
            await _platformTypeService.DeleteAsync(id);

            // Assert
            _mockUnitOfWork.Verify(u => u.PlatformTypeRepository.Delete(platformTypeToDelete), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            _mockLogger.Verify(
                l => l.LogInfo($"Platform type with id {id} was deleted successfully"), Times.Once);
        }

        [Fact]
        public async Task DeletePlatformTypeAsync_WithWrongId_ShouldThrowNotFoundException()
        {
            // Arrange
            var id = -1;
            PlatformType platformTypeToDelete = null;

            _mockUnitOfWork
                .Setup(u => u.PlatformTypeRepository
                    .GetByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(platformTypeToDelete);

            // Act
            var result = _platformTypeService.DeleteAsync(id);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task GetPlatformTypeAsync_ShouldReturnListOfGenres()
        {
            // Arrange
            var platformTypeList = new List<PlatformType> { new PlatformType() };
            var platformTypeListDTO = new List<PlatformTypeReadListDTO> { new PlatformTypeReadListDTO() };

            _mockUnitOfWork
                .Setup(u => u.PlatformTypeRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<PlatformType, bool>>>(),
                        It.IsAny<Func<IQueryable<PlatformType>, IOrderedQueryable<PlatformType>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(platformTypeList);

            _mockMapper
                .Setup(m => m.Map<IEnumerable<PlatformTypeReadListDTO>>(platformTypeList))
                .Returns(platformTypeListDTO);

            // Act
            var result = await _platformTypeService.GetAsync();

            // Assert
            _mockLogger.Verify(
                l => l.LogInfo($"Platform types were returned successfully in array size of {platformTypeListDTO.Count()}"),
                Times.Once);
            Assert.IsAssignableFrom<IEnumerable<PlatformTypeReadListDTO>>(result);
            Assert.True(result.Any());
        }

        [Fact]
        public async Task GetPlatformTypeAsync_ShouldReturnEmptyListOfGenres()
        {
            // Arrange
            var platformTypeList = new List<PlatformType>();
            var platformTypeListDTO = new List<PlatformTypeReadListDTO>();

            _mockUnitOfWork
                .Setup(u => u.PlatformTypeRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<PlatformType, bool>>>(),
                        It.IsAny<Func<IQueryable<PlatformType>, IOrderedQueryable<PlatformType>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(platformTypeList);

            _mockMapper
                .Setup(m => m.Map<IEnumerable<PlatformTypeReadListDTO>>(platformTypeList))
                .Returns(platformTypeListDTO);

            // Act
            var result = await _platformTypeService.GetAsync();

            // Assert
            _mockLogger.Verify(
                l => l.LogInfo($"Platform types were returned successfully in array size of {platformTypeListDTO.Count()}"),
                Times.Once);
            Assert.IsAssignableFrom<IEnumerable<PlatformTypeReadListDTO>>(result);
            Assert.False(result.Any());
        }

        [Fact]
        public async Task GetPlatformTypeById_WithCorrectId_ShouldReturnPlatformAndLog()
        {
            // Arrange
            var id = 1;
            var platformType = new PlatformType { Id = id };
            var platfromTypeDTO = new PlatformTypeReadDTO { Id = id };

            _mockUnitOfWork
                .Setup(u => u.PlatformTypeRepository
                    .GetByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(platformType);

            _mockMapper
                .Setup(m => m.Map<PlatformTypeReadDTO>(platformType))
                .Returns(platfromTypeDTO);

            // Act
            var result = await _platformTypeService.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PlatformTypeReadDTO>(result);
            _mockLogger.Verify(
                l => l.LogInfo($"Platform type with id {id} successfully returned"), Times.Once);
        }

        [Fact]
        public async Task GetPlatformTypeById_WithWrongId_ShouldThrowNotFoundAsync()
        {
            // Arrange
            var id = 0;
            PlatformType genre = null;

            _mockUnitOfWork
                .Setup(u => u.PlatformTypeRepository
                    .GetByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(genre);

            // Act
            var result = _platformTypeService.GetByIdAsync(id);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task UpdatePlatformTypeAsync_WithCorrectModel_ShouldUpdateAndLog()
        {
            // Arrange
            var platformTypeToUpdate = new PlatformType { Type = "test" };
            var platformTypeToUpdateDTO = new PlatformTypeUpdateDTO { Type = "test" };

            _mockUnitOfWork
                .Setup(u => u.PlatformTypeRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<PlatformType, bool>>>(),
                        It.IsAny<Func<IQueryable<PlatformType>, IOrderedQueryable<PlatformType>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<PlatformType> { platformTypeToUpdate });

            _mockMapper
                .Setup(m => m.Map(platformTypeToUpdateDTO, platformTypeToUpdate)).Verifiable();

            // Act
            await _platformTypeService.UpdateAsync(platformTypeToUpdateDTO);

            // Assert
            _mockUnitOfWork.Verify(u => u.PlatformTypeRepository.Update(platformTypeToUpdate), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            _mockLogger.Verify(
                l => l.LogInfo($"Platform type with type {platformTypeToUpdate.Type} was updated successfully"), Times.Once);
        }

        [Fact]
        public async Task UpdatePlatformTypeAsync_WithWrongModel_ShouldThrowNotFoundException()
        {
            // Arrange
            PlatformType platformTypeToUpdate = null;
            var platformTypeToUpdateDTO = new PlatformTypeUpdateDTO { Type = "wrongType" };

            _mockUnitOfWork
                .Setup(u => u.PlatformTypeRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<PlatformType, bool>>>(),
                        It.IsAny<Func<IQueryable<PlatformType>, IOrderedQueryable<PlatformType>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<PlatformType> { platformTypeToUpdate });

            // Act
            var result = _platformTypeService.UpdateAsync(platformTypeToUpdateDTO);

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
