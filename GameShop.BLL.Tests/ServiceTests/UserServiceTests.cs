using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using GameShop.BLL.DTO.UserDTOs;
using GameShop.BLL.Enums;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.BLL.Strategies.BanStrategies;
using GameShop.BLL.Strategies.Interfaces.Factories;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;
using Moq;
using Xunit;

namespace GameShop.BLL.Tests.ServiceTests
{
    public class UserServiceTests : IDisposable
    {
        private readonly Mock<IPasswordProvider> _mockPasswordProvider;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerManager> _mockLoggerManager;
        private readonly Mock<IValidator<UserCreateDTO>> _mockValidator;
        private readonly Mock<IBanFactory> _mockBanFactory;
        private readonly UserService _userService;

        private bool _disposed;

        public UserServiceTests()
        {
            _mockPasswordProvider = new Mock<IPasswordProvider>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLoggerManager = new Mock<ILoggerManager>();
            _mockValidator = new Mock<IValidator<UserCreateDTO>>();
            _mockBanFactory = new Mock<IBanFactory>();

            _userService = new UserService(
                _mockPasswordProvider.Object,
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockLoggerManager.Object,
                _mockValidator.Object,
                _mockBanFactory.Object);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task IsAnExistingUserAsync_ReturnsTrueIfUserExists()
        {
            // Arrange
            var nickName = "john.doe";
            var existingUser = new User { Id = 1, NickName = nickName };

            _mockUnitOfWork
                .Setup(u => u.UserRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<User, bool>>>(),
                        It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<User> { existingUser });

            // Act
            var result = await _userService.IsAnExistingUserAsync(nickName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsAnExistingUserAsync_ReturnsFalseIfUserDoesNotExist()
        {
            // Arrange
            var nickName = "jane.doe";

            _mockUnitOfWork
                .Setup(u => u.UserRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<User, bool>>>(),
                        It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(Enumerable.Empty<User>());

            // Act
            var result = await _userService.IsAnExistingUserAsync(nickName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task IsAnExistingUserBannedAsync_ReturnsTrueIfUserIsBanned()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var nickName = "john.doe";
            var bannedUser = new User { Id = 1, NickName = nickName, BannedTo = DateTime.UtcNow.AddHours(1) };

            _mockUnitOfWork
                .Setup(u => u.UserRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<User, bool>>>(),
                        It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<User> { bannedUser });

            // Act
            var result = await _userService.IsAnExistingUserBannedAsync(nickName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsAnExistingUserBannedAsync_ReturnsFalseIfUserIsNotBanned()
        {
            // Arrange
            var nickName = "jane.doe";
            var user = new User { Id = 2, NickName = nickName, BannedTo = DateTime.UtcNow.AddHours(-1) };

            _mockUnitOfWork
                .Setup(u => u.UserRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<User, bool>>>(),
                        It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<User> { user });

            // Act
            var result = await _userService.IsAnExistingUserBannedAsync(nickName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task IsAnExistingUserBannedAsync_ThrowsNotFoundExceptionIfUserNotFound()
        {
            // Arrange
            var nickName = "not.found";

            _mockUnitOfWork
                .Setup(u => u.UserRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<User, bool>>>(),
                        It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(Enumerable.Empty<User>());

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _userService.IsAnExistingUserBannedAsync(nickName));
        }

        [Fact]
        public async Task IsValidUserCredentialsAsync_ReturnsTrueForValidCredentials()
        {
            // Arrange
            var nickName = "john.doe";
            var password = "password123";
            var userCreateDTO = new UserCreateDTO { NickName = nickName, Password = password };
            var user = new User { Id = 1, NickName = nickName, PasswordHash = "hashed_password" };

            _mockUnitOfWork
               .Setup(u => u.UserRepository
                   .GetAsync(
                       It.IsAny<Expression<Func<User, bool>>>(),
                       It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                       It.IsAny<string>(),
                       It.IsAny<bool>()))
               .ReturnsAsync(new List<User> { user });

            _mockPasswordProvider
                .Setup(x => x
                    .IsPasswordValid(
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                .Returns(true);

            // Act
            var result = await _userService.IsValidUserCredentialsAsync(userCreateDTO);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsValidUserCredentialsAsync_ReturnsFalseForInvalidCredentials()
        {
            // Arrange
            var userCreateDTO = new UserCreateDTO { NickName = string.Empty, Password = string.Empty };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _userService.IsValidUserCredentialsAsync(userCreateDTO));
        }

        [Fact]
        public async Task IsValidUserCredentialsAsync_ReturnsFalseIfUserNotFound()
        {
            // Arrange
            var nickName = "not.found";
            var password = "password123";
            var userCreateDTO = new UserCreateDTO { NickName = nickName, Password = password };

            _mockUnitOfWork
               .Setup(u => u.UserRepository
                   .GetAsync(
                       It.IsAny<Expression<Func<User, bool>>>(),
                       It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                       It.IsAny<string>(),
                       It.IsAny<bool>()))
               .ReturnsAsync(Enumerable.Empty<User>());

            // Act
            var result = await _userService.IsValidUserCredentialsAsync(userCreateDTO);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task IsValidUserCredentialsAsync_ReturnsFalseForInvalidPassword()
        {
            // Arrange
            var nickName = "john.doe";
            var password = "invalid_password";
            var userCreateDTO = new UserCreateDTO { NickName = nickName, Password = password };
            var user = new User { Id = 1, NickName = nickName, PasswordHash = "hashed_password" };

            _mockUnitOfWork
               .Setup(u => u.UserRepository
                   .GetAsync(
                       It.IsAny<Expression<Func<User, bool>>>(),
                       It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                       It.IsAny<string>(),
                       It.IsAny<bool>()))
               .ReturnsAsync(new List<User> { user });

            _mockPasswordProvider
                .Setup(x => x
                    .IsPasswordValid(
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                .Returns(false);

            // Act
            var result = await _userService.IsValidUserCredentialsAsync(userCreateDTO);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetRoleAsync_ReturnsUserRoleName()
        {
            // Arrange
            var nickName = "john.doe";
            var roleName = "Admin";
            var user = new User { Id = 1, NickName = nickName, UserRole = new Role { Name = roleName } };

            _mockUnitOfWork
               .Setup(u => u.UserRepository
                   .GetAsync(
                       It.IsAny<Expression<Func<User, bool>>>(),
                       It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                       It.IsAny<string>(),
                       It.IsAny<bool>()))
               .ReturnsAsync(new List<User> { user });

            // Act
            var result = await _userService.GetRoleAsync(nickName);

            // Assert
            Assert.Equal(roleName, result);
        }

        [Fact]
        public async Task GetRoleAsync_ThrowsNotFoundExceptionIfUserNotFound()
        {
            // Arrange
            var nickName = "not.found";

            _mockUnitOfWork
               .Setup(u => u.UserRepository
                   .GetAsync(
                       It.IsAny<Expression<Func<User, bool>>>(),
                       It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                       It.IsAny<string>(),
                       It.IsAny<bool>()))
               .ReturnsAsync(Enumerable.Empty<User>());

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _userService.GetRoleAsync(nickName));
        }

        [Fact]
        public async Task GetIdAsync_ReturnsUserId()
        {
            // Arrange
            var nickName = "john.doe";
            var userId = 1;
            var user = new User { Id = userId, NickName = nickName };

            _mockUnitOfWork
               .Setup(u => u.UserRepository
                   .GetAsync(
                       It.IsAny<Expression<Func<User, bool>>>(),
                       It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                       It.IsAny<string>(),
                       It.IsAny<bool>()))
               .ReturnsAsync(new List<User> { user });

            // Act
            var result = await _userService.GetIdAsync(nickName);

            // Assert
            Assert.Equal(userId, result);
        }

        [Fact]
        public async Task GetIdAsync_ThrowsNotFoundExceptionIfUserNotFound()
        {
            // Arrange
            var nickName = "not.found";

            _mockUnitOfWork
               .Setup(u => u.UserRepository
                   .GetAsync(
                       It.IsAny<Expression<Func<User, bool>>>(),
                       It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                       It.IsAny<string>(),
                       It.IsAny<bool>()))
               .ReturnsAsync(Enumerable.Empty<User>());

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _userService.GetIdAsync(nickName));
        }

        [Fact]
        public async Task CreateUserAsync_CreatesUserSuccessfully()
        {
            // Arrange
            var userCreateDTO = new UserCreateDTO
            {
                NickName = "john.doe",
                Password = "password"
            };
            var user = new User
            {
                Id = 1,
                NickName = userCreateDTO.NickName,
                PasswordHash = "hashed_password",
                RoleId = 1
            };
            var role = new Role { Id = 1, Name = "User" };

            _mockMapper
                .Setup(m => m
                    .Map<User>(It.IsAny<UserCreateDTO>()))
                .Returns(user);

            _mockPasswordProvider
                .Setup(x => x.GetPasswordHash(It.IsAny<string>()))
                .Returns("hashed_password");

            _mockUnitOfWork
                .Setup(u => u.RoleRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Role, bool>>>(),
                        It.IsAny<Func<IQueryable<Role>, IOrderedQueryable<Role>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<Role> { role });

            _mockUnitOfWork
                .Setup(u => u.UserRepository
                    .Insert(It.IsAny<User>()))
                .Verifiable();

            // Act
            await _userService.CreateUserAsync(userCreateDTO);

            // Assert
            _mockUnitOfWork.Verify(u => u.UserRepository.Insert(user), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateUserWithRoleAsync_CreatesUserWithRoleSuccessfully()
        {
            // Arrange
            var userWithRoleCreateDTO = new UserCreateWithRoleDTO
            {
                NickName = "john.doe",
                Password = "password",
                RoleId = 1,
                PublisherId = 1
            };
            var user = new User { Id = 1, NickName = userWithRoleCreateDTO.NickName };
            var role = new Role { Id = userWithRoleCreateDTO.RoleId, Name = "Publisher" };
            var publisher = new Publisher { Id = 1 };

            _mockMapper
                .Setup(m => m
                    .Map<User>(It.IsAny<UserCreateDTO>()))
                .Returns(user);

            _mockPasswordProvider
                .Setup(x => x.GetPasswordHash(It.IsAny<string>()))
                .Returns("hashed_password");

            _mockUnitOfWork
                .Setup(u => u.RoleRepository
                    .GetByIdAsync(
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                .ReturnsAsync(role);

            _mockUnitOfWork
                .Setup(u => u.PublisherRepository
                    .GetByIdAsync(
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                .ReturnsAsync(publisher);

            _mockUnitOfWork
                .Setup(u => u.UserRepository
                    .Insert(It.IsAny<User>()))
                .Verifiable();

            // Act
            await _userService.CreateUserWithRoleAsync(userWithRoleCreateDTO);

            // Assert
            _mockUnitOfWork.Verify(u => u.UserRepository.Insert(user), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
            _mockLoggerManager.Verify(
                x => x.LogInfo($"User with nickname {user.NickName} created succesfully"), Times.Once);
        }

        [Fact]
        public async Task CreateUserWithRoleAsync_ThrowBadRequestException()
        {
            // Arrange
            var userWithRoleCreateDTO = new UserCreateWithRoleDTO
            {
                NickName = "john.doe",
                Password = "password",
                RoleId = 1,
                PublisherId = 1
            };
            var user = new User { Id = 1, NickName = userWithRoleCreateDTO.NickName };
            var role = new Role { Id = userWithRoleCreateDTO.RoleId, Name = "Publisher" };
            var publisher = new Publisher { Id = 1 };

            _mockMapper
                .Setup(m => m
                    .Map<User>(It.IsAny<UserCreateDTO>()))
                .Returns(user);

            _mockPasswordProvider
                .Setup(x => x.GetPasswordHash(It.IsAny<string>()))
                .Returns("hashed_password");

            _mockUnitOfWork
                .Setup(u => u.RoleRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Role, bool>>>(),
                        It.IsAny<Func<IQueryable<Role>, IOrderedQueryable<Role>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(Enumerable.Empty<Role>());

            // Act
            var result = _userService.CreateUserWithRoleAsync(userWithRoleCreateDTO);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(() => result);
        }

        [Fact]
        public async Task DeleteUserAsync_DeletesUserSuccessfully()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId };

            _mockUnitOfWork
                .Setup(u => u.UserRepository
                    .GetByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(user);

            _mockUnitOfWork
                .Setup(u => u.UserRepository
                    .Delete(It.IsAny<User>()))
                .Verifiable();

            // Act
            await _userService.DeleteUserAsync(userId);

            // Assert
            _mockUnitOfWork.Verify(u => u.UserRepository.Delete(user), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            _mockLoggerManager.Verify(
                l => l.LogInfo($"User with id {userId} was deleted successfully"), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_DeletesUserThrowNotFoundException()
        {
            // Arrange
            var userId = 1;
            User user = null;

            _mockUnitOfWork
                .Setup(u => u.UserRepository
                    .GetByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(user);

            // Act
            var result = _userService.DeleteUserAsync(userId);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task UpdateUserAsync_UpdatesUserSuccessfully()
        {
            // Arrange
            var userUpdateDTO = new UserUpdateDTO
            {
                Id = 1,
                RoleId = 2
            };
            var user = new User
            {
                Id = userUpdateDTO.Id,
                UserRole = new Role { Name = "Publisher" }
            };
            var role = new Role { Id = userUpdateDTO.RoleId, Name = "User" };

            _mockUnitOfWork
               .Setup(u => u.UserRepository
                   .GetAsync(
                       It.IsAny<Expression<Func<User, bool>>>(),
                       It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                       It.IsAny<string>(),
                       It.IsAny<bool>()))
               .ReturnsAsync(new List<User> { user });

            _mockUnitOfWork
                .Setup(u => u.RoleRepository
                    .GetByIdAsync(
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                .ReturnsAsync(role);

            _mockUnitOfWork
                .Setup(u => u.UserRepository
                    .Update(It.IsAny<User>()))
                .Verifiable();

            // Act
            await _userService.UpdateUserAsync(userUpdateDTO);

            // Assert
            _mockUnitOfWork.Verify(u => u.UserRepository.Update(user), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            _mockLoggerManager
                .Verify(l => l.LogInfo($"Genre with id {userUpdateDTO.Id} was updated successfully"), Times.Once);
        }

        [Fact]
        public async Task GetUsersAsync_ReturnsUsersSuccessfully()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, UserRole = new Role { Name = "User" } },
                new User { Id = 2, UserRole = new Role { Name = "Admin" } }
            };
            var usersDto = new List<UserReadListDTO>
            {
                new UserReadListDTO() { Id = 1 },
                new UserReadListDTO() { Id = 2 }
            };

            _mockUnitOfWork
               .Setup(u => u.UserRepository
                   .GetAsync(
                       It.IsAny<Expression<Func<User, bool>>>(),
                       It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                       It.IsAny<string>(),
                       It.IsAny<bool>()))
               .ReturnsAsync(users);

            _mockMapper
                .Setup(m => m
                    .Map<IEnumerable<UserReadListDTO>>(It.IsAny<IEnumerable<User>>()))
                .Returns(usersDto);

            // Act
            var result = await _userService.GetUsersAsync();

            // Assert
            _mockLoggerManager
                .Verify(x => x.LogInfo($"Users were returned successfully in array size of {result.Count()}"), Times.Once);
            Assert.Equal(users.Count, result.Count());
        }

        [Fact]
        public async Task BanUserAsync_BansUserSuccessfully()
        {
            // Arrange
            var userBanDTO = new UserBanDTO
            {
                NickName = "john",
                BanOption = "Hour"
            };
            var user = new User { NickName = "john" };

            _mockUnitOfWork
               .Setup(u => u.UserRepository
                   .GetAsync(
                       It.IsAny<Expression<Func<User, bool>>>(),
                       It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                       It.IsAny<string>(),
                       It.IsAny<bool>()))
               .ReturnsAsync(new List<User> { user });

            _mockBanFactory
                .Setup(bf => bf
                    .GetBanStrategy(It.IsAny<BanOptions>()))
                .Returns(new HourBanStrategy());

            // Act
            await _userService.BanUserAsync(userBanDTO);

            // Assert
            _mockUnitOfWork.Verify(u => u.UserRepository.Update(user), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
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
                _mockLoggerManager.Invocations.Clear();
                _mockValidator.Invocations.Clear();
            }

            _disposed = true;
        }
    }
}
