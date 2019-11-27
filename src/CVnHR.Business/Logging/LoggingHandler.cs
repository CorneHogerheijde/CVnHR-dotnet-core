using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CVnHR.Business.Logging
{
    public class LoggingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Log($"Request: {request.ToString()}");
            if (request.Content != null)
            {
                Log(await request.Content.ReadAsStringAsync());
            }

            try
            {
                var response = await base.SendAsync(request, cancellationToken);

                Log($"Response statuscode: {response.StatusCode}");
                if (response.Content != null)
                {
                    Log(await response.Content.ReadAsStringAsync());
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

#warning //TODO: implement better logger
        public void Log(string content)
        {
            Console.WriteLine(content);
        }
    }
}
