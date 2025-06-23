using OrderServiceApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderServiceApi.Service.Interface
{
    public interface IOrderService
    {
        Task<Order?> GetOrderAsync(Guid id);
        Task<Order> SetOrderAsync(Models.Order order);
    }
}
