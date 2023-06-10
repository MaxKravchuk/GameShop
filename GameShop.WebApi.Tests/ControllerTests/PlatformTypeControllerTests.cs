using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using GameShop.BLL.DTO.PlatformTypeDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.WebApi.Controllers;
using Moq;
using Xunit;

namespace GameShop.WebApi.Tests.ControllerTests
{
    public class PlatformTypeControllerTests
    {
        private readonly Mock<IPlatformTypeService> _mockPlatformTypeService;
        private readonly PlatformTypeController _platformTypeController;

        public PlatformTypeControllerTests()
        {
            _mockPlatformTypeService = new Mock<IPlatformTypeService>();
            _platformTypeController = new PlatformTypeController(_mockPlatformTypeService.Object);
        }

        [Fact]
        public async Task GetAllPlatformTypes_ShouldReturnListOfPlatformTypes()
        {
            // Arrange
            var platformTypesList = new List<PlatformTypeReadListDTO> { new PlatformTypeReadListDTO() };

            _mockPlatformTypeService
                .Setup(s => s
                    .GetAsync())
                .ReturnsAsync(platformTypesList);

            // Act
            var actionResult = await _platformTypeController.GetAllPlatformTypes();

            // Assert
            Assert.IsType<JsonResult<IEnumerable<PlatformTypeReadListDTO>>>(actionResult);
            Assert.NotNull(actionResult);
            _mockPlatformTypeService.Verify(s => s.GetAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllPlatformTypes_ShouldReturnEmptyListOfPlatformTypes()
        {
            // Arrange
            var platformTypesList = new List<PlatformTypeReadListDTO>();

            _mockPlatformTypeService
                .Setup(s => s
                    .GetAsync())
                .ReturnsAsync(platformTypesList);

            // Act
            var actionResult = await _platformTypeController.GetAllPlatformTypes();

            // Assert
            Assert.IsType<JsonResult<IEnumerable<PlatformTypeReadListDTO>>>(actionResult);
            Assert.NotNull(actionResult);
            _mockPlatformTypeService.Verify(s => s.GetAsync(), Times.Once);
        }
    }
}
