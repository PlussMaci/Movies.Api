using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using System.Web.Http;

namespace Movies.Api.Common.Results
{
    public class CustomResult<T> : IHttpActionResult
    {
        private readonly HttpRequestMessage _request;
        public HttpStatusCode statusCode { get; private set; }
        public T data { get; private set; }

        public CustomResult(HttpRequestMessage request, HttpStatusCode statusCode, T data)
        {
            _request = request;
            this.statusCode = statusCode;
            this.data = data;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = _request.CreateResponse<T>(statusCode, data);
            return Task.FromResult(response);
        }

        public HttpResponseMessage Execute()
        {
            return _request.CreateResponse<T>(statusCode, data);
        }
    }
}