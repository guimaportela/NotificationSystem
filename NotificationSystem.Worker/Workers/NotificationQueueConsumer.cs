using Microsoft.Extensions.Logging;
using NotificationSystem.Contracts;
using NotificationSystem.Contracts.Business;
using NotificationSystem.Contracts.Infrastructure;
using NotificationSystem.Infrastructure.Queueing;

namespace NotificationSystem.Worker.Workers
{
    public class NotificationQueueConsumer : WorkerBase<NotificationDTO>
    {
        public NotificationQueueConsumer(ILoggerFactory logger, int sleepTimeInSeconds, IMemoryQueueProvider memoryCache, INotificationBO notificationBO) : base(logger, sleepTimeInSeconds, memoryCache, notificationBO)
        {
        }

        public override Task<List<NotificationDTO>> GetItemsToProcess()
        {
            var result = new List<NotificationDTO>();

            var obj = _memoryQueueProvider.RetrieveAsync<NotificationDTO>(QueueKey.OnNotificationRetry);
            if (obj == default(NotificationDTO))
                return Task.FromResult(result);

            result.Add(obj);
            return Task.FromResult(result);
        }

        public async override Task ProcessItem(NotificationDTO item)
        {
            _logger.LogInformation($"Processing Notification from user: {item.UserId} for Type: {item.Type}");

            await _notificationBO.Send(item);
        }

        public override Task ProcessItemError(NotificationDTO item, Exception ex)
        {
            _logger.LogInformation($"Error Processing Notification from user: {item.UserId} for Type: {item.Type}");

            return Task.CompletedTask;
        }
    }
}
