namespace NotificationSystem.Common
{
    public static class NotificationRateLimits
    {
        //TO-DO: this can be dynamic, being recorded in a database
        private static readonly Dictionary<string, RateLimitConfig> RateLimits = new()
        {
            { NotificationType.Status, new RateLimitConfig(2, 1) },
            { NotificationType.News, new RateLimitConfig(1, 1440) },
            { NotificationType.Marketing, new RateLimitConfig(3, 60) }
        };

        public static RateLimitConfig GetRateLimit(string messageType)
        {
            if (!RateLimits.ContainsKey(messageType))
                throw new Exception($"Rate limit not configured for message type: {messageType}");

            return RateLimits[messageType];
        }

        public static bool CheckIfTypeIsMapped(string messageType)
        {
            return RateLimits.ContainsKey(messageType);
        }
    }

    public class RateLimitConfig
    {
        public int Limit { get; }
        public int PeriodInMinutes { get; }

        public RateLimitConfig(int limit, int periodInMinutes)
        {
            Limit = limit;
            PeriodInMinutes = periodInMinutes;
        }
    }
}
