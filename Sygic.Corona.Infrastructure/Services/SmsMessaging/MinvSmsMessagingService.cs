using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Sygic.Corona.Domain.Common;

namespace Sygic.Corona.Infrastructure.Services.SmsMessaging
{
    public class MinvSmsMessagingService : ISmsMessagingService
    {
        private readonly HttpClient client;

        public MinvSmsMessagingService(HttpClient client)
        {
            this.client = client;
        }
        public async Task SendMessageAsync(string messageText, string phoneNumber, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "");

            const string contentType = "application/soap+xml;action=\"http://schema.minv.sk/IP/ESISPZ-SMS/SmsEXT-v1/ISmsExt/SendMessage\"";
            var message = new StringContent(
                $"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:sms=\"http://schema.minv.sk/IP/ESISPZ-SMS/SmsEXT-v1\">\r\n   <soap:Header/>\r\n   <soap:Body>\r\n      <sms:SendMessage>\r\n         <sms:Destination>{phoneNumber}</sms:Destination>\r\n         <sms:Message>{messageText}</sms:Message>\r\n      </sms:SendMessage>\r\n   </soap:Body>\r\n</soap:Envelope>");
            request.Content = message;

            //var tt = client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", contentType);
            //request.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            var t = request.Headers.TryAddWithoutValidation("Content-Type", contentType);
           
            var response = await client.SendAsync(request, cancellationToken);

            var result = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                throw new DomainException("Error during sending sms.");
            }
        }
    }
}
