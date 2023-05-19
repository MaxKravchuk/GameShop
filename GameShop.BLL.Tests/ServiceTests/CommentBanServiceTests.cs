using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.Services;
using GameShop.BLL.Services.Interfaces.Utils;
using Moq;
using Xunit;

namespace BLL.Test.ServiceTests
{
    public class CommentBanServiceTests : IDisposable
    {
        private readonly CommentBanService _commentBanService;
        private readonly Mock<ILoggerManager> _mockLogger;

        private bool _disposed;

        public CommentBanServiceTests()
        {
            _mockLogger = new Mock<ILoggerManager>();

            _commentBanService = new CommentBanService(
                _mockLogger.Object);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [Fact]
        public void Ban_ShouldLog()
        {
            // Arrange
            var duration = "1day";

            _mockLogger
                .Setup(l => l
                    .LogInfo(It.IsAny<string>()))
                .Verifiable();

            // Act
            _commentBanService.Ban(duration);

            // Assert
            _mockLogger.Verify(l => l.LogInfo("Service invoked successfully"), Times.Once);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _mockLogger.Invocations.Clear();
            }

            _disposed = true;
        }
    }
}
