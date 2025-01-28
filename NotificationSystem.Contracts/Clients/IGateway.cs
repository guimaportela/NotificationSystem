namespace NotificationSystem.Contracts.Clients
{
    public interface IGateway
    {
        Task Send(string userId, string message);
    }
}
