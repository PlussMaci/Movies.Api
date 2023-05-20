using System.Net.Http;
using System.Threading.Tasks;

namespace Movies.Api.Common.Handlers
{
    public class SaveRawPostDataHandler : DelegatingHandler
    {
        public const string RawDataKey = "_rawPostData";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                System.Threading.CancellationToken cancellationToken)
        {
            request.Properties.Add(SaveRawPostDataHandler.RawDataKey, request.Content.ReadAsStringAsync().Result);
            return base.SendAsync(request, cancellationToken);
        }
    }
}