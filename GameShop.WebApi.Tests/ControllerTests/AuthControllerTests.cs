using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using GameShop.BLL.DTO.AuthDTOs;
using GameShop.BLL.DTO.UserDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.WebApi.Controllers;
using Moq;
using Xunit;

namespace GameShop.WebApi.Tests.ControllerTests
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IJwtTokenProvider> _mockJwtTokenProvider;
        private readonly Mock<IUsersTokenService> _mockUserTokenService;
        private readonly AuthController _authController;

        public AuthControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockJwtTokenProvider = new Mock<IJwtTokenProvider>();
            _mockUserTokenService = new Mock<IUsersTokenService>();

            _authController = new AuthController(
                _mockUserService.Object,
                _mockJwtTokenProvider.Object,
                _mockUserTokenService.Object);
        }

        [Fact]
        public async Task LoginAsync_ReturnsJsonResult_WhenCredentialsAreValid()
        {
            // Arrange
            var userCreateDTO = new UserCreateDTO
            {
                NickName = "john",
                Password = "password"
            };
            var authResponse = new AuthenticatedResponse
            {
                Token = "access_token",
                RefreshToken = "refresh_token"
            };

            _mockUserService
                .Setup(us => us
                    .IsValidUserCredentialsAsync(It.IsAny<UserCreateDTO>()))
                .ReturnsAsync(true);

            _mockUserService
                .Setup(us => us
                    .GetRoleAsync(It.IsAny<string>()))
                .ReturnsAsync("UserRole");

            _mockUserService
                .Setup(us => us
                    .GetIdAsync(It.IsAny<string>()))
                .ReturnsAsync(1);

            _mockJwtTokenProvider
                .Setup(jtp => jtp
                    .GetAuthenticatedResponse(
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                .Returns(authResponse);

            _mockUserTokenService
                .Setup(us => us
                    .AddUserTokenAsync(
                        It.IsAny<UserCreateDTO>()))
                .ReturnsAsync(new AuthenticatedResponse());

            // Act
            var result = await _authController.LoginAsync(userCreateDTO);

            // Assert
            Assert.IsType<JsonResult<AuthenticatedResponse>>(result);

            var jsonResult = (JsonResult<AuthenticatedResponse>)result;
            var authResponseResult = jsonResult.Content;
            Assert.Equal("access_token", authResponse.Token);
            Assert.Equal("refresh_token", authResponse.RefreshToken);
        }

        [Fact]
        public async Task LoginAsync_ThrowsBadRequestException_WhenCredentialsAreInvalid()
        {
            // Arrange
            var userCreateDTO = new UserCreateDTO
            {
                NickName = "john",
                Password = "password"
            };

            _mockUserService
                .Setup(us => us
                    .IsValidUserCredentialsAsync(It.IsAny<UserCreateDTO>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _authController.LoginAsync(userCreateDTO));
        }

        [Fact]
        public async Task RegisterAsync_ReturnsOkResult_WhenUserCreationIsSuccessful()
        {
            // Arrange
            var userCreateDTO = new UserCreateDTO
            {
                NickName = "john",
                Password = "password"
            };

            _mockUserService
                .Setup(us => us
                    .CreateUserAsync(It.IsAny<UserCreateDTO>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _authController.RegisterAsync(userCreateDTO);

            // Assert
            Assert.IsType<OkResult>(result);
            _mockUserService.Verify(us => us.CreateUserAsync(userCreateDTO), Times.Once);
        }
    }
}
