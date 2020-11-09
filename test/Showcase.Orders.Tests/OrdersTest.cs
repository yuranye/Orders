using System;
using System.Threading.Tasks;
using FluentAssertions;
using Showcase.Orders;
using Xunit;

namespace Showcase.Orders.Tests
{
    public class UnitTest1: IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;

        public UnitTest1(TestFixture fixture)
        {
            _fixture = fixture;
        }
        
        [Fact]
        public async Task ShouldCreateOrder()
        {
            var client = new OrdersService.OrdersServiceClient(_fixture.GrpcChannel);

            var placeOrderRequest = new CreateOrderRequest
            {
                Order = new Order
                {
                    Count = 10,
                    DeliveryUuid = Guid.NewGuid().ToString(),
                    ProductUuid = Guid.NewGuid().ToString(),
                },
            };

            var response = await client.CreateOrderAsync(placeOrderRequest);

            placeOrderRequest.Order.OrderUuid = response.Order.OrderUuid;//normalise response

            var persistedOrder = await client.GetOrderAsync(new GetOrderRequest
            {
                OrderUuid = response.Order.OrderUuid,
            });

            persistedOrder.Order.Should().Be(placeOrderRequest.Order);
        }
    }
}