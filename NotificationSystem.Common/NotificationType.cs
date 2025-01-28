namespace NotificationSystem.Common
{
    public static class NotificationType
    {
        public const string Status = "status";
        public const string News = "news";
        public const string Marketing = "marketing";

        //OAuth Notifications
        public const string ResetPassword = "resetPassword"; //ASSUMPTION: OAuth messages will also be send by this Service
    }
}
