using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.Services;
using GameShop.DAL.Entities;
using Xunit;
using Moq;
using GameShop.BLL.Services.Interfaces;
using AutoMapper;
using GameShop.BLL.DTO.CommentDTOs;
using GameShop.DAL.Repository.Interfaces;
using System.Linq.Expressions;
using GameShop.BLL.Exceptions;

namespace BLL.Test
{
    public class CommentServiceTests : IDisposable
    {
        private readonly Mock<ICommentService> MockService;

        private readonly Mock<IUnitOfWork> MockUnitOfWork;

        private readonly CommentService commentService;

        private readonly Mock<IMapper> MockMapper;

        private readonly Mock<ILoggerManager> MockLogger;

        private bool _disposed;

        public CommentServiceTests()
        {
            MockService = new Mock<ICommentService>();
            MockUnitOfWork = new Mock<IUnitOfWork>();
            MockMapper = new Mock<IMapper>();
            MockLogger = new Mock<ILoggerManager>();

            commentService = new CommentService(
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
        public async Task GetAllByGameKeyAsync_WithExistingGameKey_ReturnsComments()
        {
            // Arrange
            var gameKey = "game_key";
            var comments = new List<Comment> { new Comment() };
            var commentReadDTOs = new List<CommentReadDTO> { new CommentReadDTO() };

            MockUnitOfWork
                .Setup(x => x.CommentRepository.GetAsync(
                    It.IsAny<Expression<Func<Comment, bool>>>(),
                    It.IsAny<Func<IQueryable<Comment>, IOrderedQueryable<Comment>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(comments);

            MockMapper
                .Setup(x => x.Map<IEnumerable<CommentReadDTO>>(comments))
                .Returns(commentReadDTOs);

            // Act
            var result = await commentService.GetAllByGameKeyAsync(gameKey);

            // Assert
            Assert.Equal(commentReadDTOs, result);
            MockLogger.Verify(x => x.LogInfo($"Comments with game`s key {gameKey} successfully found"), Times.Once);
        }

        [Fact]
        public async Task GetAllByGameKeyAsync_WithNonExistingGameKey_ThrowsNotFoundException()
        {
            //Arrange
            var gameKey = "wrongKey";
            var comments = new List<Comment>();

            MockUnitOfWork
            .Setup(x => x.CommentRepository.GetAsync(
                    It.IsAny<Expression<Func<Comment, bool>>>(),
                    It.IsAny<Func<IQueryable<Comment>, IOrderedQueryable<Comment>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
            .ReturnsAsync(comments);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => commentService.GetAllByGameKeyAsync(gameKey));
        }

        [Fact]
        public async Task GetCommentByIdAsync_WithCorrectId_ReturnsCommet()
        {
            //Arrange
            var commentId = 1;
            var comment = new Comment();
            var commentReadDTO = new CommentReadDTO();

            MockUnitOfWork
                .Setup(x => x.CommentRepository.GetByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(comment);
            MockMapper
                .Setup(x => x.Map<CommentReadDTO>(comment))
                .Returns(commentReadDTO);

            //Act
            var result = await commentService.GetByIdAsync(commentId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<CommentReadDTO>(result);
            Assert.Equal(commentReadDTO, result);
        }

        [Fact]
        public async Task GetCommentByIdAsync_WithWrongId_ThrowNotFoundException()
        {
            //Arrange
            var commentId = 0;
            Comment comment = null;

            MockUnitOfWork
                .Setup(x => x.CommentRepository.GetByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(comment);

            //Act
            var result = commentService.GetByIdAsync(commentId);

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateCommentAndLogInfo()
        {
            // Arrange
            var commentCreateDto = new CommentCreateDTO() { GameKey = "gameKey"};
            var game = new Game() { Id = 1, Key = "gameKey"};
            var comment = new Comment();

            MockMapper.Setup(m => m.Map<Comment>(commentCreateDto))
                      .Returns(comment);

            MockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<Game> { game });

            MockUnitOfWork
                .Setup(u => u.CommentRepository.Insert(comment)).Verifiable();

            // Act
            await commentService.CreateAsync(commentCreateDto);

            // Assert
            MockUnitOfWork.Verify(u => u.CommentRepository.Insert(comment), Times.Once);
            MockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            MockLogger.Verify(l => l
            .LogInfo($"Comment for game`s key {commentCreateDto.GameKey} created successfully"), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowBadRequestException()
        {
            // Arrange
            var commentCreateDto = new CommentCreateDTO() { GameKey = string.Empty };
            var game = new Game();
            var comment = new Comment();

            MockMapper.Setup(m => m.Map<Comment>(commentCreateDto))
                      .Returns(comment);

            MockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<Game> { game });

            MockUnitOfWork
                .Setup(u => u.CommentRepository.Insert(comment)).Verifiable();

            // Act
            var result = commentService.CreateAsync(commentCreateDto);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(() => result);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowNotFoundException()
        {
            // Arrange
            var commentCreateDto = new CommentCreateDTO() { GameKey = "black" };
            var game = new Game() { Key = "white"};
            var comment = new Comment();

            MockMapper.Setup(m => m.Map<Comment>(commentCreateDto))
                      .Returns(comment);

            MockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<Game>());

            MockUnitOfWork
                .Setup(u => u.CommentRepository.Insert(comment)).Verifiable();

            // Act
            var result = commentService.CreateAsync(commentCreateDto);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task DeleteAsync_WithCorrectId_ShouldDelete()
        {
            //Arrange
            var commentId = 1;
            var comment = new Comment() { Id = commentId};

            MockUnitOfWork
                .Setup(u => u.CommentRepository
                    .GetByIdAsync(
                        It.Is<int>(id => id == commentId),
                        It.IsAny<string>()))
                .ReturnsAsync(comment);

            MockUnitOfWork
                .Setup(u => u.CommentRepository
                    .Delete(comment))
                .Verifiable();

            //Act
            await commentService.DeleteAsync(commentId);

            //Assert
            MockUnitOfWork.Verify(u => u.CommentRepository.Delete(comment), Times.Once);
            MockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            MockLogger.Verify(l => l
            .LogInfo($"Comment with id {commentId} deleted successfully"), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithWrongId_ShouldThrowNotFoundException()
        {
            //Arrange
            var commentId = 0;
            Comment comment = null;

            MockUnitOfWork
                .Setup(u => u.CommentRepository
                    .GetByIdAsync(
                        It.Is<int>(id => id == commentId),
                        It.IsAny<string>()))
                .ReturnsAsync(comment);


            //Act
            var result = commentService.DeleteAsync(commentId);

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }
    }
}
