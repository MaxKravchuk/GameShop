using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using GameShop.BLL.DTO.RoleDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;
using Moq;
using Xunit;

namespace GameShop.BLL.Tests.ServiceTests
{
    public class RoleServiceTests : IDisposable
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerManager> _mockLogger;
        private readonly Mock<IValidator<RoleCreateDTO>> _mockValidator;
        private readonly RoleService _roleService;

        private bool _disposed;

        public RoleServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerManager>();
            _mockValidator = new Mock<IValidator<RoleCreateDTO>>();

            _roleService = new RoleService(
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
        public async Task CreateRoleAsync_CreatesRoleSuccessfully()
        {
            // Arrange
            var roleCreateDTO = new RoleCreateDTO
            {
                Name = "Admin",
            };

            var role = new Role
            {
                Name = roleCreateDTO.Name,
            };

            _mockMapper
                .Setup(m => m
                    .Map<Role>(It.IsAny<RoleCreateDTO>()))
                .Returns(role);

            _mockUnitOfWork
                .Setup(u => u.RoleRepository
                    .Insert(It.IsAny<Role>()))
                .Verifiable();

            // Act
            await _roleService.CreateRoleAsync(roleCreateDTO);

            // Assert
            _mockUnitOfWork.Verify(u => u.RoleRepository.Insert(role), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
            _mockLogger.Verify(x => x.LogInfo($"Role - {roleCreateDTO.Name} was created successfully"), Times.Once);
        }

        [Fact]
        public async Task DeleteRoleAsync_DeletesRoleSuccessfully()
        {
            // Arrange
            var roleId = 1;
            var roleToDelete = new Role { Id = roleId };
            var users = new List<User>
            {
                new User { Id = 1, UserRole = roleToDelete },
                new User { Id = 2, UserRole = roleToDelete }
            };

            _mockUnitOfWork
                .Setup(u => u.RoleRepository
                    .GetByIdAsync(
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                .ReturnsAsync(roleToDelete);

            _mockUnitOfWork
                .Setup(u => u.UserRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<User, bool>>>(),
                        It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(users);

            // Act
            await _roleService.DeleteRoleAsync(roleId);

            // Assert
            foreach (var user in users)
            {
                Assert.Null(user.RoleId);
                _mockUnitOfWork.Verify(u => u.UserRepository.Update(user), Times.Once);
            }

            _mockUnitOfWork.Verify(u => u.RoleRepository.HardDelete(roleToDelete), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
            _mockLogger.Verify(x => x.LogInfo($"Role with id {roleId} was deleted successfully"), Times.Once);
        }

        [Fact]
        public async Task DeleteRoleAsync_ShouldThrowNotFoundException()
        {
            // Arrange
            var roleId = 1;
            Role roleToDelete = null;

            _mockUnitOfWork
                .Setup(u => u.RoleRepository
                    .GetByIdAsync(
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                .ReturnsAsync(roleToDelete);

            // Act
            var result = _roleService.DeleteRoleAsync(roleId);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task GetRolesAsync_ReturnsRolesSuccessfully()
        {
            // Arrange
            var roles = new List<Role>
            {
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
            };

            var roleDTOs = new List<RoleReadListDTO>
            {
                new RoleReadListDTO { Id = 1, Name = "Admin" },
                new RoleReadListDTO { Id = 2, Name = "User" }
            };

            _mockUnitOfWork
                .Setup(u => u.RoleRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Role, bool>>>(),
                        It.IsAny<Func<IQueryable<Role>, IOrderedQueryable<Role>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(roles);

            _mockMapper
                .Setup(m => m.Map<IEnumerable<RoleReadListDTO>>(It.IsAny<IEnumerable<Role>>()))
                .Returns(roleDTOs);

            // Act
            var result = await _roleService.GetRolesAsync();

            // Assert
            Assert.Equal(roleDTOs, result);
            _mockLogger
                .Verify(l => l.LogInfo($"Roles were returned successfully in array size of {roleDTOs.Count}"), Times.Once);
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
                _mockValidator.Invocations.Clear();
            }

            _disposed = true;
        }
    }
}
