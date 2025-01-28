using NotificationSystem.Contracts.Clients;
using System.Net;

namespace NotificationSystem.Clients
{
    public class Gateway : IGateway
    {
        public async Task Send(string userId, string message)
        {
            Console.WriteLine($"Sending message to user {userId}");

            await Task.Delay(10); //Faking a real server connection

            Console.WriteLine($"Message sent to user {userId}");
        }
    }
}
