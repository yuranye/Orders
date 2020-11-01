using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Interceptor to the grpc http client to set version as default test server set it to 1.1 and grpc requires http2 to work
/// </summary>
public class ResponseVersionHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        response.Version = request.Version;

        return response;
    }
}