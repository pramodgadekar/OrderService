using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderServiceApi.Models
{
    public class Order
    {
        public Guid OrderId { get; set; } = Guid.NewGuid();
        public string CustomerId { get; set; } = string.Empty;
        public List<OrderItem> Items { get; set; } = new();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public OrderType Status { get; set; } = OrderType.Pending;
    }

    public class OrderItem
    {
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
    /// <summary>
    /// Order Type - Pending, Confirmed, Shipped, Failed, Cancelled
    /// </summary>
    public enum OrderType {
        Pending,
        Confirmed,
        Shipped,
        Failed,
        Cancelled
    }

}
