using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using GameShop.BLL.DTO.CommentDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services.Interfaces;
using GameShop.WebApi.Controllers;
using Moq;
using Xunit;

namespace GameShop.WebApi.Tests
{
    public class CommentControllerTests
    {
        private readonly Mock<ICommentService> _mockCommentService;
        private readonly CommentController _commentController;

        public CommentControllerTests()
        {
            _mockCommentService = new Mock<ICommentService>();
            _commentController = new CommentController(_mockCommentService.Object);
        }

        [Fact]
        public async Task CreateCommentAsync_WithCorrectModel_ShouldReturnStatus200Ok()
        {
            // Arrange
            var commentCreateDTO = new CommentCreateDTO();

            _mockCommentService
                .Setup(s => s
                    .CreateAsync(It.IsAny<CommentCreateDTO>())).Verifiable();

            // Act
            var actionResult = await _commentController.CreateCommentAsync(commentCreateDTO);

            // Assert
            Assert.IsType<OkResult>(actionResult);
            Assert.NotNull(actionResult);
            _mockCommentService.Verify(x => x.CreateAsync(commentCreateDTO), Times.Once);
        }

        [Fact]
        public async Task CreateCommentAsync_WithWrongModel_ShouldReturnStatus500NotFound()
        {
            // Arrange
            var commentCreateDTO = new CommentCreateDTO();

            _mockCommentService
                .Setup(s => s
                    .CreateAsync(It.IsAny<CommentCreateDTO>())).ThrowsAsync(new NotFoundException());

            // Act
            var actionResult = _commentController.CreateCommentAsync(commentCreateDTO);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => actionResult);
        }

        [Fact]
        public async Task CreateCommentAsync_WithWrongModel_ShouldReturnStatus500BadRequest()
        {
            // Arrange
            var commentCreateDTO = new CommentCreateDTO();

            _mockCommentService
                .Setup(s => s
                    .CreateAsync(It.IsAny<CommentCreateDTO>())).ThrowsAsync(new BadRequestException());

            // Act
            var actionResult = _commentController.CreateCommentAsync(commentCreateDTO);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(() => actionResult);
        }

        [Fact]
        public async Task GetAllCommentsByGameKey_WithCorrectGameKey_ShouldReturnComments()
        {
            // Arrange
            var gameKey = "key";
            var listOfComments = new List<CommentReadDTO> { new CommentReadDTO() };

            _mockCommentService
                .Setup(s => s
                    .GetAllByGameKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(listOfComments);

            // Act
            var actionResult = await _commentController.GetAllCommentsByGameKey(gameKey);

            // Assert
            Assert.IsType<JsonResult<IEnumerable<CommentReadDTO>>>(actionResult);
            Assert.NotNull(actionResult);
            _mockCommentService.Verify(s => s.GetAllByGameKeyAsync(gameKey), Times.Once);
        }

        [Fact]
        public async Task GetAllCommentsByGameKey_ShouldReturnComments()
        {
            // Arrange
            var gameKey = "bad_key";
            var listOfComments = new List<CommentReadDTO> { new CommentReadDTO() };

            _mockCommentService
                .Setup(s => s
                    .GetAllByGameKeyAsync(It.IsAny<string>()))
                .ThrowsAsync(new NotFoundException());

            // Act
            var actionResult = _commentController.GetAllCommentsByGameKey(gameKey);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => actionResult);
        }
    }
}
