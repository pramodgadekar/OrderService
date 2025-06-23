using Confluent.Kafka;
using OrderServiceApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderServiceApi.KafkaProvider
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<string, string> _producer;

        public KafkaProducer()
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task PublishOrderCreatedEvent(Order order)
        {
            var msg = new Message<string, string>
            {
                Key = order.OrderId.ToString(),
                Value = JsonSerializer.Serialize(new { order.OrderId, order.Timestamp })
            };
            await _producer.ProduceAsync("orders.created", msg);
        }
    }
}
