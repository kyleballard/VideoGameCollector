using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace VideoGameCollector.Web
{
    public class ValidateUserKeyHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {

            if (!request.Headers.Contains("user-key") || 
                string.IsNullOrEmpty(request.Headers.GetValues("user-key").FirstOrDefault()))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("You must supply a http header value for user-key")
                };
            }     

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
