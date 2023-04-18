using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using GameShop.BLL.DTO.CommentDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services;
using GameShop.BLL.Services.Interfaces;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;
using Moq;
using Xunit;

namespace GameShop.BLL.Tests
{
    public class CommentServiceTests : IDisposable
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly CommentService _commentService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerManager> _mockLogger;

        private bool _disposed;

        public CommentServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerManager>();

            _commentService = new CommentService(
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
        public async Task GetAllByGameKeyAsync_WithExistingGameKey_ReturnsComments()
        {
            // Arrange
            var gameKey = "game_key";
            var comments = new List<Comment> { new Comment() };
            var commentReadDTOs = new List<CommentReadDTO> { new CommentReadDTO() };

            _mockUnitOfWork
                .Setup(x => x.CommentRepository.GetAsync(
                    It.IsAny<Expression<Func<Comment, bool>>>(),
                    It.IsAny<Func<IQueryable<Comment>, IOrderedQueryable<Comment>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(comments);

            _mockMapper
                .Setup(x => x.Map<IEnumerable<CommentReadDTO>>(comments))
                .Returns(commentReadDTOs);

            // Act
            var result = await _commentService.GetAllByGameKeyAsync(gameKey);

            // Assert
            Assert.Equal(commentReadDTOs, result);
            _mockLogger.Verify(x => x.LogInfo($"Comments with game`s key {gameKey} successfully found"), Times.Once);
        }

        [Fact]
        public async Task GetAllByGameKeyAsync_WithNonExistingGameKey_ThrowsNotFoundException()
        {
            // Arrange
            var gameKey = "wrongKey";
            var comments = new List<Comment>();

            _mockUnitOfWork
            .Setup(x => x.CommentRepository.GetAsync(
                    It.IsAny<Expression<Func<Comment, bool>>>(),
                    It.IsAny<Func<IQueryable<Comment>, IOrderedQueryable<Comment>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
            .ReturnsAsync(comments);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _commentService.GetAllByGameKeyAsync(gameKey));
        }

        [Fact]
        public async Task GetCommentByIdAsync_WithCorrectId_ReturnsCommet()
        {
            // Arrange
            var commentId = 1;
            var comment = new Comment();
            var commentReadDTO = new CommentReadDTO();

            _mockUnitOfWork
                .Setup(x => x.CommentRepository.GetByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(comment);
            _mockMapper
                .Setup(x => x.Map<CommentReadDTO>(comment))
                .Returns(commentReadDTO);

            // Act
            var result = await _commentService.GetByIdAsync(commentId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CommentReadDTO>(result);
            Assert.Equal(commentReadDTO, result);
        }

        [Fact]
        public async Task GetCommentByIdAsync_WithWrongId_ThrowNotFoundException()
        {
            // Arrange
            var commentId = 0;
            Comment comment = null;

            _mockUnitOfWork
                .Setup(x => x.CommentRepository.GetByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(comment);

            // Act
            var result = _commentService.GetByIdAsync(commentId);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateCommentAndLogInfo()
        {
            // Arrange
            var commentCreateDto = new CommentCreateDTO() { GameKey = "gameKey" };
            var game = new Game() { Id = 1, Key = "gameKey" };
            var comment = new Comment();

            _mockMapper.Setup(m => m.Map<Comment>(commentCreateDto))
                      .Returns(comment);

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<Game> { game });

            _mockUnitOfWork
                .Setup(u => u.CommentRepository.Insert(comment)).Verifiable();

            // Act
            await _commentService.CreateAsync(commentCreateDto);

            // Assert
            _mockUnitOfWork.Verify(u => u.CommentRepository.Insert(comment), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            _mockLogger.Verify(
                l => l.LogInfo($"Comment for game`s key {commentCreateDto.GameKey} created successfully"), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowBadRequestException()
        {
            // Arrange
            var commentCreateDto = new CommentCreateDTO() { GameKey = string.Empty };
            var game = new Game();
            var comment = new Comment();

            _mockMapper.Setup(m => m.Map<Comment>(commentCreateDto))
                      .Returns(comment);

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<Game> { game });

            _mockUnitOfWork
                .Setup(u => u.CommentRepository.Insert(comment)).Verifiable();

            // Act
            var result = _commentService.CreateAsync(commentCreateDto);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(() => result);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowNotFoundException()
        {
            // Arrange
            var commentCreateDto = new CommentCreateDTO() { GameKey = "black" };
            var game = new Game() { Key = "white" };
            var comment = new Comment();

            _mockMapper.Setup(m => m.Map<Comment>(commentCreateDto))
                      .Returns(comment);

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<Game>());

            _mockUnitOfWork
                .Setup(u => u.CommentRepository.Insert(comment)).Verifiable();

            // Act
            var result = _commentService.CreateAsync(commentCreateDto);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task DeleteAsync_WithCorrectId_ShouldDelete()
        {
            // Arrange
            var commentId = 1;
            var comment = new Comment() { Id = commentId };

            _mockUnitOfWork
                .Setup(u => u.CommentRepository
                    .GetByIdAsync(
                        It.Is<int>(id => id == commentId),
                        It.IsAny<string>()))
                .ReturnsAsync(comment);

            _mockUnitOfWork
                .Setup(u => u.CommentRepository
                    .Delete(comment))
                .Verifiable();

            // Act
            await _commentService.DeleteAsync(commentId);

            // Assert
            _mockUnitOfWork.Verify(u => u.CommentRepository.Delete(comment), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            _mockLogger.Verify(
                l => l.LogInfo($"Comment with id {commentId} deleted successfully"), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithWrongId_ShouldThrowNotFoundException()
        {
            // Arrange
            var commentId = 0;
            Comment comment = null;

            _mockUnitOfWork
                .Setup(u => u.CommentRepository
                    .GetByIdAsync(
                        It.Is<int>(id => id == commentId),
                        It.IsAny<string>()))
                .ReturnsAsync(comment);

            // Act
            var result = _commentService.DeleteAsync(commentId);

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
