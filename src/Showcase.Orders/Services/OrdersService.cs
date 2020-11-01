using System;
using System.Threading.Tasks;
using Grpc.Core;
using Showcase.Orders;

namespace Showcase.Orders.Services
{
    public class OrdersService: Showcase.Orders.OrdersService.OrdersServiceBase
    {
        public override Task<PlaceOrderResponse> PlaceOrder(PlaceOrderRequest request, ServerCallContext context)
        {
            return Task.FromResult(new PlaceOrderResponse
            {
                Order = new Order
                {
                    Count = request.Order.Count,
                    DeliveryUuid = request.Order.DeliveryUuid,
                    ProductUuid = request.Order.ProductUuid
                }
            });
        }
    }
}