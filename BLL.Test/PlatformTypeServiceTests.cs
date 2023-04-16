using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using GameShop.BLL.DTO.GenreDTOs;
using GameShop.BLL.DTO.PlatformTypeDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services;
using GameShop.BLL.Services.Interfaces;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;
using Moq;
using Xunit;

namespace BLL.Test
{
    public class PlatformTypeServiceTests : IDisposable
    {
        private readonly Mock<IPlatformTypeService> MockService;
        private readonly Mock<IUnitOfWork> MockUnitOfWork;
        private readonly PlatformTypeService PlatformTypeService;
        private readonly Mock<IMapper> MockMapper;
        private readonly Mock<ILoggerManager> MockLogger;

        private bool _disposed;

        public PlatformTypeServiceTests()
        {
            MockService = new Mock<IPlatformTypeService>();
            MockUnitOfWork = new Mock<IUnitOfWork>();
            MockMapper = new Mock<IMapper>();
            MockLogger = new Mock<ILoggerManager>();

            PlatformTypeService = new PlatformTypeService(
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
        public async Task CreatePlatformTypeAsync_WithCorrectModel_ShouldCreateAndLog()
        {
            // Arrange
            var platformTypeToAddDTO = new PlatformTypeCreateDTO();
            var platformToAdd = new PlatformType();

            MockMapper
                .Setup(m => m.Map<PlatformType>(platformTypeToAddDTO)).Returns(platformToAdd);

            MockUnitOfWork
                .Setup(u => u.PlatformTypeRepository
                    .Insert(platformToAdd)).Verifiable();

            // Act
            await PlatformTypeService.CreateAsync(platformTypeToAddDTO);

            // Assert
            MockUnitOfWork.Verify(u => u.PlatformTypeRepository.Insert(platformToAdd), Times.Once);
            MockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            MockLogger.Verify(l => l
            .LogInfo($"Platform type with type {platformTypeToAddDTO.Type} was created successfully"), Times.Once);
        }

        [Fact]
        public async Task DeletePlatformTypeAsync_WithCorrectId_ShouldDeleteAndLog()
        {
            // Arrange
            var id = 1;
            var platformTypeToDelete = new PlatformType { Id = 1 };

            MockUnitOfWork
                .Setup(u => u.PlatformTypeRepository
                    .GetByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(platformTypeToDelete);

            MockUnitOfWork
                .Setup(u => u.PlatformTypeRepository
                    .Delete(platformTypeToDelete))
                .Verifiable();

            // Act
            await PlatformTypeService.DeleteAsync(id);

            // Assert
            MockUnitOfWork.Verify(u => u.PlatformTypeRepository.Delete(platformTypeToDelete), Times.Once);
            MockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            MockLogger.Verify(l => l
            .LogInfo($"Platform type with id {id} was deleted successfully"), Times.Once);
        }

        [Fact]
        public async Task DeletePlatformTypeAsync_WithWrongId_ShouldThrowNotFoundException()
        {
            // Arrange
            var id = -1;
            PlatformType platformTypeToDelete = null;

            MockUnitOfWork
                .Setup(u => u.PlatformTypeRepository
                    .GetByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(platformTypeToDelete);

            // Act
            var result = PlatformTypeService.DeleteAsync(id);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task GetPlatformTypeAsync_ShouldReturnListOfGenres()
        {
            // Arrange
            var platformTypeList = new List<PlatformType> { new PlatformType() };
            var platformTypeListDTO = new List<PlatformTypeReadListDTO> { new PlatformTypeReadListDTO() };

            MockUnitOfWork
                .Setup(u => u.PlatformTypeRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<PlatformType, bool>>>(),
                        It.IsAny<Func<IQueryable<PlatformType>, IOrderedQueryable<PlatformType>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(platformTypeList);

            MockMapper
                .Setup(m => m.Map<IEnumerable<PlatformTypeReadListDTO>>(platformTypeList))
                .Returns(platformTypeListDTO);

            // Act
            var result = await PlatformTypeService.GetAsync();

            // Assert
            MockLogger.Verify(
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

            MockUnitOfWork
                .Setup(u => u.PlatformTypeRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<PlatformType, bool>>>(),
                        It.IsAny<Func<IQueryable<PlatformType>, IOrderedQueryable<PlatformType>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(platformTypeList);

            MockMapper
                .Setup(m => m.Map<IEnumerable<PlatformTypeReadListDTO>>(platformTypeList))
                .Returns(platformTypeListDTO);

            // Act
            var result = await PlatformTypeService.GetAsync();

            // Assert
            MockLogger.Verify(
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

            MockUnitOfWork
                .Setup(u => u.PlatformTypeRepository
                    .GetByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(platformType);

            MockMapper
                .Setup(m => m.Map<PlatformTypeReadDTO>(platformType))
                .Returns(platfromTypeDTO);

            // Act
            var result = await PlatformTypeService.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PlatformTypeReadDTO>(result);
            MockLogger.Verify(l => l.LogInfo(
                $"Platform type with id {id} successfully returned"), Times.Once);
        }

        [Fact]
        public async Task GetPlatformTypeById_WithWrongId_ShouldThrowNotFoundAsync()
        {
            // Arrange
            var id = 0;
            PlatformType genre = null;

            MockUnitOfWork
                .Setup(u => u.PlatformTypeRepository
                    .GetByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(genre);

            // Act
            var result = PlatformTypeService.GetByIdAsync(id);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task UpdatePlatformTypeAsync_WithCorrectModel_ShouldUpdateAndLog()
        {
            // Arrange
            var platformTypeToUpdate = new PlatformType { Type = "test" };
            var platformTypeToUpdateDTO = new PlatformTypeUpdateDTO { Type = "test" };

            MockUnitOfWork
                .Setup(u => u.PlatformTypeRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<PlatformType, bool>>>(),
                        It.IsAny<Func<IQueryable<PlatformType>, IOrderedQueryable<PlatformType>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<PlatformType> { platformTypeToUpdate });

            MockMapper
                .Setup(m => m.Map(platformTypeToUpdateDTO, platformTypeToUpdate)).Verifiable();

            // Act
            await PlatformTypeService.UpdateAsync(platformTypeToUpdateDTO);

            // Assert
            MockUnitOfWork.Verify(u => u.PlatformTypeRepository.Update(platformTypeToUpdate), Times.Once);
            MockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            MockLogger.Verify(l => l
                .LogInfo($"Platform type with type {platformTypeToUpdate.Type} was updated successfully"), Times.Once);
        }

        [Fact]
        public async Task UpdatePlatformTypeAsync_WithWrongModel_ShouldThrowNotFoundException()
        {
            // Arrange
            PlatformType platformTypeToUpdate = null; 
            var platformTypeToUpdateDTO = new PlatformTypeUpdateDTO { Type = "wrongType" };

            MockUnitOfWork
                .Setup(u => u.PlatformTypeRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<PlatformType, bool>>>(),
                        It.IsAny<Func<IQueryable<PlatformType>, IOrderedQueryable<PlatformType>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<PlatformType> { platformTypeToUpdate });

            // Act
            var result = PlatformTypeService.UpdateAsync(platformTypeToUpdateDTO);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }
    }
}
