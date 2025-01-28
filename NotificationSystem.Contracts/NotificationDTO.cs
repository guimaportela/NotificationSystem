namespace NotificationSystem.Contracts
{
    public class NotificationDTO
    {
        public string Type { get; set; }
        public string UserId { get; set; }
        public string? Message { get; set; }
    }
}
