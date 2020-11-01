using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Showcase.Orders;
using Xunit;

namespace Showcase.Orders.Tests
{
    public class TestFixture: IAsyncLifetime
    {
        private WebApplicationFactory<Startup> _factory;
        public GrpcChannel GrpcChannel { get; private set; }

        public Task InitializeAsync()
        {
            //This flag is needed because grpc call have to be unencrypted (in dev env only)
            //https://github.com/grpc/grpc-dotnet/issues/626
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            _factory = new WebApplicationFactory<Startup>();
            var client = _factory.CreateDefaultClient(new ResponseVersionHandler());
            GrpcChannel = GrpcChannel.ForAddress(client.BaseAddress, new GrpcChannelOptions
            {
                HttpClient = client
            });
            
            return Task.CompletedTask;
        }
        

        public Task DisposeAsync()
        {
            _factory.Dispose();
            GrpcChannel.Dispose();
            return Task.CompletedTask;
        }
    }
}