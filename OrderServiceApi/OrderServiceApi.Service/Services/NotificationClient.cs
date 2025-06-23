using OrderServiceApi.Models;
using OrderServiceApi.Service.Interface;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OrderServiceApi.Service.Services
{
    public class NotificationClient : INotificationClient
    {
        private readonly HttpClient _httpClient;

        public NotificationClient(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        public async Task SendOrderNotificationAsync(Order order)
        {
            var policy = Policy.Handle<HttpRequestException>().WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(2));
            var notificationServiceUrl = "http://localhost:5001/api/Notification/Notify";// read from config file
            await policy.ExecuteAsync(async () =>
            {
                var res = await _httpClient.PostAsJsonAsync(notificationServiceUrl, order);
                res.EnsureSuccessStatusCode();
            });
        }
    }

}
