using OrderServiceApi.Models;
using OrderServiceApi.Service.Interface;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderServiceApi.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly StackExchange.Redis.IDatabase _redis;

        public OrderService(IConnectionMultiplexer redis)
        {
            _redis = redis.GetDatabase();
        }

        public async Task<Models.Order?> GetOrderAsync(Guid id)
        {
            var cached = await _redis.StringGetAsync($"order:{id}");
            if (cached.IsNullOrEmpty) return null; //If Null fetchfrom Database

            return JsonSerializer.Deserialize<Models.Order>(cached!);
        }

        public async Task<Models.Order> SetOrderAsync(Models.Order order)
        {
            var json = JsonSerializer.Serialize(order);
            // Store Order In DataBase
            await _redis.StringSetAsync($"order:{order.OrderId}", json, TimeSpan.FromMinutes(5)); // Catch Invalidation Fetched from Config
            return order;
        }
    }
}
