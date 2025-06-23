namespace NotificationServiceApi.Web.Models
{
    public class OrderNotification
    {
        public Guid OrderId { get; set; }
        public string CustomerId { get; set; } = string.Empty;
        public List<OrderItem> Items { get; set; } = new();
        public DateTime Timestamp { get; set; }
        public OrderType Status { get; set; }
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
