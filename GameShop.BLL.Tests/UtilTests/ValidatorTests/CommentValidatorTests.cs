using FluentAssertions;
using GameShop.BLL.DTO.CommentDTOs;
using GameShop.BLL.Services.Utils.Validators;
using Xunit;

namespace GameShop.BLL.Tests.UtilTests.ValidatorTests
{
    public class CommentValidatorTests
    {
        private readonly CommentCreateDtoValidator _validationRules;

        public CommentValidatorTests()
        {
            _validationRules = new CommentCreateDtoValidator();
        }

        [Fact]
        public void CommentCreateDTOValidator_Validate_ValidInput_ShouldPass()
        {
            // Arrange
            var commentCreateDTO = new CommentCreateDTO
            {
                Name = "John Doe",
                Body = "This is a comment",
                GameKey = "1234"
            };

            // Act
            var result = _validationRules.Validate(commentCreateDTO);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("", "This is a comment", "1234", "Comment must have author name")]
        [InlineData("John Doe", "", "1234", "Comment body cannot be empty")]
        [InlineData("John Doe", "This is a comment", "", "Comment must have game key")]
        public void CommentCreateDTOValidator_Validate_InvalidInput_ShouldFail(
            string name, string body, string gameKey, string errorMessage)
        {
            // Arrange
            var commentCreateDTO = new CommentCreateDTO
            {
                Name = name,
                Body = body,
                GameKey = gameKey
            };

            // Act
            var result = _validationRules.Validate(commentCreateDTO);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == errorMessage);
        }
    }
}
