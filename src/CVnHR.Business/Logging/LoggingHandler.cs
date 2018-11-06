using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CVnHR.Business.Logging
{
#warning //TODO: remove all Console.WriteLine!

    public class LoggingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Request:");
            Console.WriteLine(request.ToString());
            if (request.Content != null)
            {
                Console.WriteLine(await request.Content.ReadAsStringAsync());
            }
            Console.WriteLine();

            try
            {
                var result = base.SendAsync(request, cancellationToken);
                var response = await result;

                Console.WriteLine($"Response statuscode: {response.StatusCode}");
                if (response.Content != null)
                {
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
                Console.WriteLine();

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
