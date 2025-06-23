using AutoFixture;
using Moq;
using OrderServiceApi.Service.Services;
using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace OrderServiceApi.Tests.Services
{
    public class OrderServiceTests
    {
        private MockRepository mockRepository;

        private Mock<IConnectionMultiplexer> mockConnectionMultiplexer;
        private Mock<IDatabase> mockCache;

        public OrderServiceTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockConnectionMultiplexer = this.mockRepository.Create<IConnectionMultiplexer>();
            this.mockCache = new Mock<IDatabase>();
            this.mockConnectionMultiplexer.Setup(x => x.GetDatabase(It.IsAny<int>(),It.IsAny<object>())).Returns(mockCache.Object);
        }

        private OrderService CreateService()
        {
            return new OrderService(
                this.mockConnectionMultiplexer.Object);
        }

        [Fact]
        public async Task GetOrderAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            Guid id = default(global::System.Guid);
            Models.Order order = new Fixture().Create<Models.Order>();
            var json = JsonSerializer.Serialize(order);
            this.mockCache.Setup(x=> x.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(json);

            // Act
            var result = await service.GetOrderAsync(
                id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.OrderId, result.OrderId);
            this.mockRepository.Verify();
        }

        
    }
}
