using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Village.Games
{
    public class WrappedHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var httpClient = new HttpClient();
            var result = httpClient.SendAsync(request.Clone(), cancellationToken);
            return result;
        }

    }
}
