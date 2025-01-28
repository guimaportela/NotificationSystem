namespace NotificationSystem.Exceptions
{
    public class RateLimitExceededException : Exception
    {
        public RateLimitExceededException() { }
        public RateLimitExceededException(string notificationType, string userId)
            : base($"Notification blocked. Type: '{notificationType}', UserId: '{userId}'. Timestamp: {DateTime.UtcNow}.") { }
    }
}
