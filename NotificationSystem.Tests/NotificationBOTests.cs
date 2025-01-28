using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NotificationSystem.Business.Business;
using NotificationSystem.Contracts;
using NotificationSystem.Contracts.Business;
using NotificationSystem.Contracts.Clients;
using NotificationSystem.Contracts.Infrastructure;
using NotificationSystem.Exceptions;

namespace NotificationSystem.Tests
{
    public class NotificationBOTests
    {
        //Clients
        private readonly Mock<IGateway> _mockGateway;
        //Infrastructure
        private readonly Mock<IMemoryCacheProvider> _mockMemoryCacheProvider;
        private readonly Mock<IMemoryQueueProvider> _mockMemoryQueueProvider;
        //Loggers
        private readonly Mock<ILogger<NotificationBO>> _loggerNotificationBO;
        //Business
        private readonly INotificationBO _notificationBO;
        //Notification
        private readonly NotificationDTO _notification;

        public NotificationBOTests()
        {
            //Clients
            _mockGateway = new Mock<IGateway>();
            //Infrastructure
            _mockMemoryCacheProvider = new Mock<IMemoryCacheProvider>();
            _mockMemoryQueueProvider = new Mock<IMemoryQueueProvider>();
            //Loggers
            _loggerNotificationBO = new Mock<ILogger<NotificationBO>>();
            //Business
            _notificationBO = new NotificationBO(_loggerNotificationBO.Object, _mockMemoryCacheProvider.Object, _mockGateway.Object, _mockMemoryQueueProvider.Object);
            //Notification
            _notification = new NotificationDTO()
            {
                Type = "status",
                UserId = "123",
                Message = ""
            };
        }

        [Fact]
        public async Task Send_ShouldReturnSuccessResult_WhenNotifierReturnsSuccess()
        {
            _mockGateway
                .Setup(g => g.Send(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            Func<Task> act = async () => await _notificationBO.Send(
                _notification.Type, _notification.UserId, _notification.Message);

            // Then
            await act.Should().NotThrowAsync();

            _mockGateway.Verify(g => g.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Send_ShouldThrowException_WhenRateLimitIsExceeded()
        {
            _mockMemoryCacheProvider
                .Setup(c => c.RetrieveAsync<Queue<DateTime>>(It.IsAny<string>()))
                .Returns(new Queue<DateTime>(new[] { DateTime.UtcNow.AddSeconds(-30), DateTime.UtcNow }));

            // Act
            Func<Task> act = async () => await _notificationBO.Send(
                _notification.Type, _notification.UserId, _notification.Message);

            // Then
            await act.Should().ThrowAsync<RateLimitExceededException>();

            _mockGateway.Verify(g => g.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}