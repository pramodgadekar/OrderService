using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderServiceApi.Service.Handler;
using OrderServiceApi.Service.Interface;

namespace OrderServiceApi.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IOrderService _cacheService;

        public OrderController(IMediator mediator, IOrderService cacheService)
        {
            _mediator = mediator;
            _cacheService = cacheService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetOrderById), new { id = result?.OrderId }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _cacheService.GetOrderAsync(id);
            if (order != null) return Ok(order);

            return NotFound();
        }
    }
}
