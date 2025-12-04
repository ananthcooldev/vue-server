using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Text;
using VueNetCrud.Server.Middleware;
using Xunit;

namespace VueNetCrud.Server.Tests.Middleware
{
    public class ErrorHandlerMiddlewareTests
    {
        private readonly Mock<ILogger<ErrorHandlerMiddleware>> _mockLogger;
        private readonly ErrorHandlerMiddleware _middleware;

        public ErrorHandlerMiddlewareTests()
        {
            _mockLogger = new Mock<ILogger<ErrorHandlerMiddleware>>();
            _middleware = new ErrorHandlerMiddleware(
                context => Task.CompletedTask,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task Invoke_WithNoException_ShouldNotThrow()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var middleware = new ErrorHandlerMiddleware(
                context => Task.CompletedTask,
                _mockLogger.Object
            );

            // Act
            var act = async () => await middleware.Invoke(context);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Invoke_WithException_ShouldLogError()
        {
            // Arrange
            var exception = new Exception("Test exception");
            var context = new DefaultHttpContext();
            var middleware = new ErrorHandlerMiddleware(
                context => throw exception,
                _mockLogger.Object
            );

            // Act
            var act = async () => await middleware.Invoke(context);

            // Assert
            await act.Should().ThrowAsync<Exception>();
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }

        [Fact]
        public async Task Invoke_WithException_ShouldRethrowException()
        {
            // Arrange
            var exception = new Exception("Test exception");
            var context = new DefaultHttpContext();
            var middleware = new ErrorHandlerMiddleware(
                context => throw exception,
                _mockLogger.Object
            );

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => middleware.Invoke(context));
        }

        [Fact]
        public async Task Invoke_WithCustomException_ShouldLogAndRethrow()
        {
            // Arrange
            var exception = new InvalidOperationException("Custom exception");
            var context = new DefaultHttpContext();
            var middleware = new ErrorHandlerMiddleware(
                context => throw exception,
                _mockLogger.Object
            );

            // Act & Assert
            var thrownException = await Assert.ThrowsAsync<InvalidOperationException>(
                () => middleware.Invoke(context));

            thrownException.Message.Should().Be("Custom exception");
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }
    }
}

