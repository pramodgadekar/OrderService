using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationServiceApi.Web.Models;

namespace NotificationServiceApi.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        [Route("Notify")]
        [HttpPost]
        public IActionResult Notify([FromBody] OrderNotification notification)
        {
            Console.WriteLine($"[NotificationService] Received notification for Order {notification.OrderId} at {notification.Timestamp}");
            return Ok();
        }
    }
}
