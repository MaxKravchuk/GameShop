using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;
using Moq;
using Xunit;

namespace GameShop.BLL.Tests.ServiceTests
{
    public class UsersTokenServiceTests : IDisposable
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly UsersTokenService _usersTokenService;

        private bool _disposed;

        public UsersTokenServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            _usersTokenService = new UsersTokenService(
                _mockUnitOfWork.Object);
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
            var user = new User { Id = 1, NickName = nickName };
            var userToken = new UserTokens { UserId = user.Id, RefreshToken = "oldRefreshToken" };

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
            var newRefreshToken = "newRefreshToken";
            await _usersTokenService.UpdateUserTokenAsync(nickName, newRefreshToken);

            // Assert
            _mockUnitOfWork.Verify(u => u.UserTokensRepository.Update(It.IsAny<UserTokens>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);

            Assert.Equal(newRefreshToken, userToken.RefreshToken);
        }

        [Fact]
        public async Task GetRefreshTokenAsync_ReturnsUserToken()
        {
            // Arrange
            var oldRefreshToken = "oldRefreshToken";
            var userToken = new UserTokens
            {
                RefreshToken = oldRefreshToken,
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7),
                User = new User { NickName = "john" }
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
            var result = await _usersTokenService.GetRefreshTokenAsync(oldRefreshToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(oldRefreshToken, result.RefreshToken);
            Assert.Equal(userToken.RefreshTokenExpiryTime, result.RefreshTokenExpiryTime);
            Assert.Equal(userToken.User.NickName, result.UserNickName);
        }

        [Fact]
        public async Task GetRefreshTokenAsync_ThrowsNotFoundException_WhenTokenNotFound()
        {
            // Arrange
            var oldRefreshToken = "oldRefreshToken";

            _mockUnitOfWork
                .Setup(u => u.UserTokensRepository
                    .GetAsync(
                       It.IsAny<Expression<Func<UserTokens, bool>>>(),
                       It.IsAny<Func<IQueryable<UserTokens>, IOrderedQueryable<UserTokens>>>(),
                       It.IsAny<string>(),
                       It.IsAny<bool>()))
                .ReturnsAsync(Enumerable.Empty<UserTokens>());

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _usersTokenService.GetRefreshTokenAsync(oldRefreshToken));
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
