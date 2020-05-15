using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Sygic.Corona.QuarantineApi.Extensions
{
    public static class HttpRequestExtensions
    {
        public static async Task<TRequest> DeserializeJsonBody<TRequest>(this HttpRequest httpRequest)
        {
            var requestBody = await new StreamReader(httpRequest.Body).ReadToEndAsync();
            return JsonConvert.DeserializeObject<TRequest>(requestBody);
        }
    }
}