using Newtonsoft.Json;
using NotificationSystem.Contracts.Clients;
using System.Net;
using System.Net.Http;
using System.Text;

namespace NotificationSystem.Clients
{
    public class Gateway : IGateway
    {
        private readonly HttpClient _httpClient;
        public Gateway()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://c2c8fec4-2bf8-4447-b616-db9c4b05d7ea.mock.pstmn.io"),
            };
        }

        public async Task Send(string userId, string messageToSend)
        {
            using StringContent encodedContent = new(messageToSend, Encoding.UTF8, "application/text");

            Console.WriteLine($"Sending message to user {userId}");
            await Task.Delay(5); //Faking a real server connection
            using HttpResponseMessage response = await _httpClient.PostAsync($"api/v1/gateway/message/{userId}", encodedContent);

            response.EnsureSuccessStatusCode();
            Console.WriteLine($"Message sent to user {userId}");
        }
    }
}
