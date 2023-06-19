using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.AuthDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;
using Moq;
using Xunit;

namespace GameShop.BLL.Tests.ServiceTests
{
    public class UsersTokenServiceTests : IDisposable
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IJwtTokenProvider> _mockJwtProvider;
        private readonly UsersTokenService _usersTokenService;

        private bool _disposed;

        public UsersTokenServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockJwtProvider = new Mock<IJwtTokenProvider>();

            _usersTokenService = new UsersTokenService(
                _mockUnitOfWork.Object,
                _mockJwtProvider.Object);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task AddUserTokenAsync_AddsNewToken()
        {
            // Arrange
            var nickName = "john";
            var refreshToken = "refreshToken";
            var user = new User { Id = 1, NickName = "john" };

            _mockUnitOfWork
                .Setup(u => u.UserTokensRepository
                    .GetAsync(
                       It.IsAny<Expression<Func<UserTokens, bool>>>(),
                       It.IsAny<Func<IQueryable<UserTokens>, IOrderedQueryable<UserTokens>>>(),
                       It.IsAny<string>(),
                       It.IsAny<bool>()))
                .ReturnsAsync(Enumerable.Empty<UserTokens>());

            _mockUnitOfWork
                .Setup(u => u.UserRepository
                    .GetAsync(
                       It.IsAny<Expression<Func<User, bool>>>(),
                       It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                       It.IsAny<string>(),
                       It.IsAny<bool>()))
                .ReturnsAsync(new List<User> { user });

            // Act
            await _usersTokenService.AddUserTokenAsync(nickName, refreshToken);

            // Assert
            _mockUnitOfWork.Verify(u => u.UserTokensRepository.Insert(It.IsAny<UserTokens>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteUserTokenAsync_DeletesUserToken()
        {
            // Arrange
            var nickName = "john";
            var user = new User { Id = 1, NickName = nickName };
            var userToken = new UserTokens { UserId = user.Id, RefreshToken = "refreshToken" };

            _mockUnitOfWork
                .Setup(u => u.UserTokensRepository
                    .GetAsync(
                       It.IsAny<Expression<Func<UserTokens, bool>>>(),
                       It.IsAny<Func<IQueryable<UserTokens>, IOrderedQueryable<UserTokens>>>(),
                       It.IsAny<string>(),
                       It.IsAny<bool>()))
                .ReturnsAsync(new List<UserTokens> { userToken });

            _mockUnitOfWork
                .Setup(u => u.UserRepository
                    .GetAsync(
                       It.IsAny<Expression<Func<User, bool>>>(),
                       It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                       It.IsAny<string>(),
                       It.IsAny<bool>()))
                .ReturnsAsync(new List<User> { user });

            // Act
            await _usersTokenService.DeleteUserTokenAsync(nickName);

            // Assert
            _mockUnitOfWork.Verify(u => u.UserTokensRepository.Update(It.IsAny<UserTokens>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
            Assert.Null(userToken.RefreshToken);
            Assert.Equal(default(DateTime), userToken.RefreshTokenExpiryTime);
        }

        [Fact]
        public async Task UpdateUserTokenAsync_UpdatesUserToken()
        {
            // Arrange
            var nickName = "john";
            var role = new Role { Name = "test" };
            var user = new User { Id = 1, NickName = nickName, UserRole = role };
            var userToken = new UserTokens
            {
                UserId = user.Id,
                RefreshToken = "oldRefreshToken",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddSeconds(30)
            };
            var authResponce = new AuthenticatedResponse { Token = "token", RefreshToken = "refToken" };

            _mockUnitOfWork
                .Setup(u => u.UserTokensRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<UserTokens, bool>>>(),
                        It.IsAny<Func<IQueryable<UserTokens>, IOrderedQueryable<UserTokens>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<UserTokens> { userToken });

            _mockUnitOfWork
                .Setup(u => u.UserRepository
                    .GetAsync(
                       It.IsAny<Expression<Func<User, bool>>>(),
                       It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                       It.IsAny<string>(),
                       It.IsAny<bool>()))
                .ReturnsAsync(new List<User> { user });

            _mockJwtProvider
                .Setup(jwt => jwt
                    .GetAuthenticatedResponse(
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                .Returns(authResponce);

            _mockUnitOfWork
                .Setup(u => u.UserTokensRepository
                    .Update(
                        It.IsAny<UserTokens>()))
                .Verifiable();

            // Act
            await _usersTokenService.UpdateUserTokenAsync("oldRefreshToken");

            // Assert
            _mockUnitOfWork.Verify(u => u.UserTokensRepository.Update(It.IsAny<UserTokens>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateUserTokenAsync_ShouldThrowNotFoundExceptionForToken()
        {
            // Arrange
            var nickName = "john";
            var role = new Role { Name = "test" };
            var user = new User { Id = 1, NickName = nickName, UserRole = role };
            var userToken = new UserTokens
            {
                UserId = user.Id,
                RefreshToken = "oldRefreshToken",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddSeconds(30)
            };

            _mockUnitOfWork
                .Setup(u => u.UserTokensRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<UserTokens, bool>>>(),
                        It.IsAny<Func<IQueryable<UserTokens>, IOrderedQueryable<UserTokens>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(Enumerable.Empty<UserTokens>());

            // Act
            var result = _usersTokenService.UpdateUserTokenAsync("oldRefreshToken");

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task UpdateUserTokenAsync_ShouldThrowBadRequestException()
        {
            // Arrange
            var nickName = "john";
            var role = new Role { Name = "test" };
            var user = new User { Id = 1, NickName = nickName, UserRole = role };
            var userToken = new UserTokens
            {
                UserId = user.Id,
                RefreshToken = "oldRefreshToken",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddSeconds(-30)
            };

            _mockUnitOfWork
                .Setup(u => u.UserTokensRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<UserTokens, bool>>>(),
                        It.IsAny<Func<IQueryable<UserTokens>, IOrderedQueryable<UserTokens>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<UserTokens> { userToken });

            // Act
            var result = _usersTokenService.UpdateUserTokenAsync("oldRefreshToken");

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(() => result);
        }

        [Fact]
        public async Task UpdateUserTokenAsync_ShouldThrowNotFoundExceptionForUser()
        {
            // Arrange
            var nickName = "john";
            var role = new Role { Name = "test" };
            var user = new User { Id = 1, NickName = nickName, UserRole = role };
            var userToken = new UserTokens
            {
                UserId = user.Id,
                User = user,
                RefreshToken = "oldRefreshToken",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddSeconds(30)
            };

            _mockUnitOfWork
               .Setup(u => u.UserTokensRepository
                   .GetAsync(
                       It.IsAny<Expression<Func<UserTokens, bool>>>(),
                       It.IsAny<Func<IQueryable<UserTokens>, IOrderedQueryable<UserTokens>>>(),
                       It.IsAny<string>(),
                       It.IsAny<bool>()))
               .ReturnsAsync(new List<UserTokens> { userToken });

            _mockUnitOfWork
                .Setup(u => u.UserRepository
                    .GetAsync(
                       It.IsAny<Expression<Func<User, bool>>>(),
                       It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                       It.IsAny<string>(),
                       It.IsAny<bool>()))
                .ReturnsAsync(Enumerable.Empty<User>());

            // Act
            var result = _usersTokenService.UpdateUserTokenAsync("oldRefreshToken");

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
            }

            _disposed = true;
        }
    }
}
