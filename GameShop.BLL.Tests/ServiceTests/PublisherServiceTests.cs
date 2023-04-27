using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using GameShop.BLL.DTO.PublisherDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;
using Moq;
using Xunit;

namespace BLL.Test.ServiceTests
{
    public class PublisherServiceTests : IDisposable
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly PublisherService _publisherService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerManager> _mockLogger;
        private readonly Mock<IValidator<PublisherCreateDTO>> _mockValidator;

        private bool _disposed;

        public PublisherServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerManager>();
            _mockValidator = new Mock<IValidator<PublisherCreateDTO>>();

            _publisherService = new PublisherService(
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
        public async Task CreatePublisherAsync_ShouldInsertAndLog()
        {
            // Arrange
            var publisherCreateDTO = new PublisherCreateDTO();
            var publisher = new Publisher();

            _mockMapper.Setup(m => m.Map<Publisher>(It.IsAny<PublisherCreateDTO>())).Verifiable();

            _mockUnitOfWork
                .Setup(u => u.PublisherRepository
                    .Insert(It.IsAny<Publisher>())).Verifiable();

            // Act
            await _publisherService.CreatePublisherAsync(publisherCreateDTO);

            // Assert
            _mockUnitOfWork.Verify(u => u.PublisherRepository.Insert(publisher), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            _mockLogger.Verify(
                l => l.LogInfo($"Publisher created successfully"), Times.Once);
        }

        [Fact]
        public async Task GetPublisherByCompanyNameAsync_WithCorrectCompanyName_ShouldReturnPublisher()
        {
            // Arrange
            var companyName = "test";
            var publisher = new Publisher() { CompanyName = companyName };
            var publishers = new List<Publisher> { publisher };
            var publisherReadDTO = new PublisherReadDTO { CompanyName = companyName };

            _mockUnitOfWork.Setup(u => u.PublisherRepository
                .GetAsync(
                    It.IsAny<Expression<Func<Publisher, bool>>>(),
                    It.IsAny<Func<IQueryable<Publisher>, IOrderedQueryable<Publisher>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(publishers);

            _mockMapper.Setup(m => m.Map<PublisherReadDTO>(It.IsAny<Publisher>())).Returns(publisherReadDTO);

            // Act
            var result = await _publisherService.GetPublisherByCompanyNameAsync("test");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test", result.CompanyName);
        }

        [Fact]
        public async Task GetPublisherByCompanyNameAsync_ThrowsNotFoundException_WhenPublisherDoesNotExist()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.PublisherRepository
                .GetAsync(
                    It.IsAny<Expression<Func<Publisher, bool>>>(),
                    It.IsAny<Func<IQueryable<Publisher>, IOrderedQueryable<Publisher>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(new List<Publisher>());

            // Act + Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _publisherService
            .GetPublisherByCompanyNameAsync("Nonexistent Publisher"));
        }

        [Fact]
        public async Task GetAllPublishersAsync_ShouldReturnListOfublishers()
        {
            // Arrange
            var publishers = new List<Publisher> { new Publisher() };
            var publisherReadListDTO = new List<PublisherReadListDTO> { new PublisherReadListDTO() };

            _mockUnitOfWork.Setup(u => u.PublisherRepository
                .GetAsync(
                    It.IsAny<Expression<Func<Publisher, bool>>>(),
                    It.IsAny<Func<IQueryable<Publisher>, IOrderedQueryable<Publisher>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(publishers);

            _mockMapper.Setup(m => m.Map<IEnumerable<PublisherReadListDTO>>(It.IsAny<Publisher>()))
                .Returns(publisherReadListDTO);

            // Act
            var result = await _publisherService.GetAllPublishersAsync();

            // Assert
            Assert.NotNull(result);
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
