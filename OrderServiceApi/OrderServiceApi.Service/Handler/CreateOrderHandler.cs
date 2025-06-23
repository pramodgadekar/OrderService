using MediatR;
using Microsoft.VisualBasic;
using OrderServiceApi.KafkaProvider;
using OrderServiceApi.Models;
using OrderServiceApi.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderServiceApi.Service.Handler
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Order>
    {
        private readonly INotificationClient _notification;
        private readonly IKafkaProducer _kafka;
        private readonly IOrderService _orderService;

        public CreateOrderHandler(INotificationClient notification, IKafkaProducer kafka, IOrderService orderService)
        {
            _notification = notification;
            _kafka = kafka;
            _orderService = orderService;
        }

        public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                CustomerId = request.CustomerId,
                Items = request.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList(),
                Timestamp = DateTime.UtcNow,
                Status = OrderType.Confirmed,
            };

            await _notification.SendOrderNotificationAsync(order);
            //await _kafka.PublishOrderCreatedEvent(order);

            return await _orderService.SetOrderAsync(order);
        }
    }
}
