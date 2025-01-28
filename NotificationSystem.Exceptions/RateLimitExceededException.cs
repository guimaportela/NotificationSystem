using NotificationSystem.Contracts;

namespace NotificationSystem.Exceptions
{
    public class RateLimitExceededException : Exception
    {
        public RateLimitExceededException() { }
        public RateLimitExceededException(NotificationDTO notificationDTO)
            : base($"Notification blocked. Type: '{notificationDTO.Type}', UserId: '{notificationDTO.UserId}'. Timestamp: {DateTime.UtcNow}.") { }
    }
}
