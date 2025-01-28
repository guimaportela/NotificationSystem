using AutoBogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NotificationSystem.Business.Business;
using NotificationSystem.Business.Services;
using NotificationSystem.Common;
using NotificationSystem.Contracts;
using NotificationSystem.Contracts.Business;
using NotificationSystem.Contracts.Clients;
using NotificationSystem.Contracts.Infrastructure;
using NotificationSystem.Exceptions;

namespace NotificationSystem.Tests
{
    public class NotificationServiceTests
    {
        //Clients
        private readonly Mock<IGateway> _mockGateway;
        //Infrastructure
        private readonly Mock<ICacheProvider> _mockCacheProvider;
        //Loggers
        private readonly Mock<ILogger<NotificationBO>> _loggerNotificationBO;
        //Business
        private readonly INotificationBO _notificationBO;
        //Notification
        private readonly NotificationDTO _notification;

        public NotificationServiceTests()
        {
            //Clients
            _mockGateway = new Mock<IGateway>();
            //Infrastructure
            _mockCacheProvider = new Mock<ICacheProvider>();
            //Loggers
            _loggerNotificationBO = new Mock<ILogger<NotificationBO>>();
            //Business
            _notificationBO = new NotificationBO(_loggerNotificationBO.Object, _mockCacheProvider.Object, _mockGateway.Object);
            //Notification
            _notification = new NotificationDTO()
            {
                Type = "status",
                UserId = "123",
                Message = ""
            };
        }

        [Fact]
        public async Task Send_ShouldThrowException_WhenRateLimitIsExceeded()
        {
            var notification = AutoFaker.Generate<NotificationDTO>();

            _mockCacheProvider
                .Setup(c => c.RetrieveAsync<Queue<DateTime>>(It.IsAny<string>()))
                .Returns(new Queue<DateTime>(new[] { DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow }));

            // Act
            Func<Task> act = async () => await _notificationBO.Send(
                _notification.Type, _notification.UserId, _notification.Message);

            // Then
            await act.Should().ThrowAsync<RateLimitExceededException>();

            _mockGateway.Verify(g => g.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}