using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using GameShop.BLL.DTO.PublisherDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services.Interfaces;
using GameShop.WebApi.Controllers;
using Moq;
using Xunit;

namespace GameShop.WebApi.Tests.ControllerTests
{
    public class PublisherControllerTests
    {
        private readonly Mock<IPublisherService> _mockPublisherService;
        private readonly PublisherController _publisherController;

        public PublisherControllerTests()
        {
            _mockPublisherService = new Mock<IPublisherService>();
            _publisherController = new PublisherController(_mockPublisherService.Object);
        }

        [Fact]
        public async Task CreatePublisher_ShouldCreatePublisher()
        {
            // Arrange
            var publisherCreateDTO = new PublisherCreateDTO();

            _mockPublisherService
                .Setup(s => s
                    .CreatePublisherAsync(It.IsAny<PublisherCreateDTO>()))
                .Verifiable();

            // Act
            var actionResult = await _publisherController.CreatePublisherAsync(publisherCreateDTO);

            // Assert
            Assert.IsType<OkResult>(actionResult);
            Assert.NotNull(actionResult);
            _mockPublisherService.Verify(x => x.CreatePublisherAsync(publisherCreateDTO), Times.Once);
        }

        [Fact]
        public async Task GetPublisherByCompanyName_WithCorrectCompanyName_ShouldReturnPublisher()
        {
            // Arrange
            var companyName = "test";
            var publisherReadDTO = new PublisherReadDTO { CompanyName = companyName };

            _mockPublisherService
                .Setup(s => s
                    .GetPublisherByCompanyNameAsync(It.IsAny<string>()))
                .ReturnsAsync(publisherReadDTO);

            // Act
            var actionResult = await _publisherController.GetPublisherByCompanyNameAsync(companyName);

            // Assert
            Assert.IsType<JsonResult<PublisherReadDTO>>(actionResult);
            Assert.NotNull(actionResult);
            _mockPublisherService.Verify(s => s.GetPublisherByCompanyNameAsync(companyName), Times.Once);
        }

        [Fact]
        public async Task GetPublisherByCompanyName_WithWrongCompanyName_ShouldReturnPublisher()
        {
            // Arrange
            var companyName = "test";

            _mockPublisherService
                .Setup(s => s
                    .GetPublisherByCompanyNameAsync(It.IsAny<string>()))
                .ThrowsAsync(new NotFoundException());

            // Act
            var actionResult = _publisherController.GetPublisherByCompanyNameAsync(companyName);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => actionResult);
        }

        [Fact]
        public async Task GetAllPublishers_ShouldReturnListOfPublishers()
        {
            // Arrange
            var publisherReadDTO = new List<PublisherReadListDTO> { new PublisherReadListDTO() };

            _mockPublisherService
                .Setup(s => s
                    .GetAllPublishersAsync())
                .ReturnsAsync(publisherReadDTO);

            // Act
            var actionResult = await _publisherController.GetAllPublishersAsync();

            // Assert
            Assert.IsType<JsonResult<IEnumerable<PublisherReadListDTO>>>(actionResult);
            Assert.NotNull(actionResult);
            _mockPublisherService.Verify(s => s.GetAllPublishersAsync(), Times.Once);
        }
    }
}
