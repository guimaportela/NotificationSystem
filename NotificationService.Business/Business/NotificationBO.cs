using Microsoft.Extensions.Logging;
using NotificationSystem.Common;
using NotificationSystem.Contracts.Business;
using NotificationSystem.Contracts.Clients;
using NotificationSystem.Contracts.Infrastructure;
using NotificationSystem.Exceptions;

namespace NotificationSystem.Business.Business
{
    public class NotificationBO : INotificationBO
    {
        private readonly ILogger<NotificationBO> _logger;
        private readonly ICacheProvider _memoryCacheProvider;
        private readonly IGateway _gateway;

        private static string GetUserNotificationsKey(string type, string userId) => $"{type}:{userId}";

        public NotificationBO(ILogger<NotificationBO> logger, ICacheProvider memoryCacheProvider, IGateway gateway)
        {
            _logger = logger;
            _memoryCacheProvider = memoryCacheProvider;
            _gateway = gateway;
        }

        public async Task Send(string type, string userId, string message)
        {
            try
            {
                //Getting last notifications cached in time window
                var notificationKey = GetUserNotificationsKey(type, userId);
                var notificationsInWindow = _memoryCacheProvider.RetrieveAsync<Queue<DateTime>>(notificationKey) ?? new Queue<DateTime>();

                if (IsRateLimited(type, userId, notificationsInWindow))
                {
                    OldestNotificationHandler(notificationKey, notificationsInWindow);

                    throw new RateLimitExceededException(type, userId);
                }

                await _gateway.Send(userId, message);
                _logger.LogInformation($"Notification sent to user '{userId}' for type '{type}'.");

                NotificationsHistoryHandler(notificationKey, notificationsInWindow);
            }
            catch (Exception e) when (e is NotImplementedException || e is RateLimitExceededException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Internal server error");
                throw;
            }
        }

        public bool IsRateLimited(string type, string userId, Queue<DateTime> notificationsInWindow)
        {
            if (!NotificationRateLimits.CheckIfTypeIsMapped(type))
                throw new NotImplementedException($"Type '{type}' is invalid");

            //Data
            var rateLimit = NotificationRateLimits.GetRateLimit(type).Limit;
            var timeWindowInMinutes = NotificationRateLimits.GetRateLimit(type).PeriodInMinutes;

            //If there is no history cached, there is no reason to block it
            if (notificationsInWindow.Count() == 0) return false;

            //Calculate the time span between the last notification registered and UTCNow
            var lastNotificationInWindow = notificationsInWindow.Peek();
            TimeSpan ts = DateTime.UtcNow - lastNotificationInWindow;
            if ((ts.TotalMinutes <= timeWindowInMinutes) && notificationsInWindow.Count() < rateLimit) return false;

            _logger.LogError("[RateLimitExceeded] Notification blocked. Type: '{NotificationType}', " +
                "UserId: '{UserId}', Limit: {RateLimit}, Period: {PeriodInMinutes} minutes. " +
                "Current Notifications in Window: {CurrentCount}. Timestamp: {Timestamp}.",
                type,
                userId,
                rateLimit,
                timeWindowInMinutes,
                notificationsInWindow.Count,
                DateTime.UtcNow);

            return true;
        }

        public void NotificationsHistoryHandler(string notificationKey, Queue<DateTime> notificationsInWindow)
        {
            //After successfully send the e-mail, we will store this in the notification in cached history
            notificationsInWindow.Enqueue(DateTime.UtcNow);
            _memoryCacheProvider.StoreAsync(notificationKey, notificationsInWindow);

            //Then remove any old notification
            OldestNotificationHandler(notificationKey, notificationsInWindow);
        }

        public void OldestNotificationHandler(string notificationKey, Queue<DateTime> notificationsInWindow)
        {
            //Data
            var timeWindowInMinutes = NotificationRateLimits.GetRateLimit(NotificationType.Status).PeriodInMinutes;

            //Checking the if the first notification in saved history is over the established time window
            var firstNotificationInWindow = notificationsInWindow.Peek();
            TimeSpan ts = DateTime.UtcNow - firstNotificationInWindow;

            if (ts.TotalMinutes <= timeWindowInMinutes)
                return;

            //If it is older than the time window, it will be removed
            notificationsInWindow.Dequeue();
            _memoryCacheProvider.StoreAsync(notificationKey, notificationsInWindow);
        }
    }
}
