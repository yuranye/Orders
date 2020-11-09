using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Showcase.Orders.Services
{
    public class OrdersService: Showcase.Orders.OrdersService.OrdersServiceBase
    {
        private readonly string _connectionString;

        private const string SelectOrderSql =
            @"SELECT 
                ORDER_UUID::text as OrderUuid,
                DELIVERY_UUID::text as DeliveryUuid,
                PRODUCT_UUID::text as ProductUuid,
                COUNT
            FROM ORDERS
            WHERE ORDER_UUID = @OrderUuid";
        
        private const string SelectManyOrdersSql =
            @"SELECT 
                ORDER_UUID::text as OrderUuid,
                DELIVERY_UUID::text as DeliveryUuid,
                PRODUCT_UUID::text as ProductUuid,
                COUNT
            FROM ORDERS
            WHERE ORDER_UUID IN @OrdersUuid";
        
        private const string InsertOrderSql =
            @"INSERT INTO ORDERS VALUES (@OrderUuid, @ProductUuid, @DeliveryUuid, @Count);";        
        
        private const string DeleteOrderSql =
            @"DELETE FROM ORDERS WHERE ORDER_UUID = @OrderUuid;";
        
        private const string UpdateOrderSql =
            @"UPDATE orders
            /**set**/
            WHERE uuid = @OrderUuid";

        public OrdersService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Postgres");
        }
        
        public override async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request, ServerCallContext context)
        {
            request.Order.OrderUuid = Guid.NewGuid().ToString();
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(InsertOrderSql, new
            {
                OrderUuid = Guid.Parse(request.Order.OrderUuid),
                ProductUuid = Guid.Parse(request.Order.ProductUuid),
                DeliveryUuid = Guid.Parse(request.Order.DeliveryUuid),
                Count = (int)request.Order.Count,
            });

            return new CreateOrderResponse
            {
                Order = request.Order,
            };
        }

        public override async Task<UpdateOrderResponse> UpdateOrder(UpdateOrderRequest request, ServerCallContext context)
        {
            var builder = new PartialUpdateSqlBuilder();
            var sql = builder.AddTemplate(UpdateOrderSql, new
            {
                request.Order.OrderUuid,
            });
            
            if (request.FieldMask.Paths.Contains(nameof(UpdateOrderRequest.Order.DeliveryUuid)))
            {
                builder.Set("PRODUCT_UUID = @ProductUuid", Guid.Parse(request.Order.ProductUuid));
            }
            if (request.FieldMask.Paths.Contains(nameof(UpdateOrderRequest.Order.ProductUuid)))
            {
                builder.Set("DELIVERY_UUID = @DeliveryUuid", Guid.Parse(request.Order.DeliveryUuid));
            }
            if (request.FieldMask.Paths.Contains(nameof(UpdateOrderRequest.Order.Count)))
            {
                builder.Set("COUNT = @Count", request.Order.Count);
            }

            await using var connection = new NpgsqlConnection(_connectionString);

            await connection.ExecuteAsync(sql.RawSql, sql.Parameters);

            return new UpdateOrderResponse
            {
                Order = request.Order,
            };
        }

        public override async Task<Empty> DeleteOrder(DeleteOrderRequest request, ServerCallContext context)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            await connection.ExecuteAsync(DeleteOrderSql, new
            {
                OrderUuid = Guid.Parse(request.OrderUuid),
            });
            
            return new Empty();
        }

        public override async Task<GetOrderResponse> GetOrder(GetOrderRequest request, ServerCallContext context)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            var order = await connection.QueryAsync<Order>(SelectOrderSql, new
            {
                OrderUuid = Guid.Parse(request.OrderUuid),
            });
            
            return new GetOrderResponse
            {
                Order = order.Single(),
            };
        }

        public override async Task<ListOrdersResponse> ListOrders(ListOrdersRequest request, ServerCallContext context)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            var orders = await connection.QueryAsync<Order>(SelectManyOrdersSql, new
            {
                OrderUuids = request.OrderUuids.Select(Guid.Parse).ToList(),
            });
            
            return new ListOrdersResponse
            {
                Orders =
                {
                    orders,
                },
            };
        }
    }
}