using MediatR;
using OrderServiceApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderServiceApi.Service.Handler
{
    public record CreateOrderCommand(string CustomerId, List<OrderItem> Items) : IRequest<Order>;
}
