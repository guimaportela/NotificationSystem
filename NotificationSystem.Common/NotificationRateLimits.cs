namespace NotificationSystem.Common
{
    public static class NotificationRateLimits
    {
        //TO-DO: this can be dynamic, being recorded in a database
        private static readonly Dictionary<string, RateLimitConfig> RateLimits = new()
        {
            { NotificationType.Status, new RateLimitConfig(2, 1) },
            { NotificationType.News, new RateLimitConfig(1, 1440) },
            { NotificationType.Marketing, new RateLimitConfig(3, 60) },
            { NotificationType.ResetPassword, new RateLimitConfig(5, 1, true) }
        };

        public static RateLimitConfig GetRateLimitByType(string messageType)
        {
            if (!RateLimits.ContainsKey(messageType))
                throw new Exception($"Rate limit not configured for message type: {messageType}"); //ASSUMPTION: Only mapped notifications will be send

            return RateLimits[messageType];
        }

        public static Dictionary<string, RateLimitConfig> GetRateLimits()
        {
            return RateLimits;
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
        public bool IsReriable { get; }

        public RateLimitConfig(int limit, int periodInMinutes, bool isReriable = false)
        {
            Limit = limit;
            PeriodInMinutes = periodInMinutes;
            IsReriable = isReriable;
        }
    }
}
