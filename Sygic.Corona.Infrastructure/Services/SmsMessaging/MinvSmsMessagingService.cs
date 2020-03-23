using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Sygic.Corona.Domain.Common;
using Sygic.Corona.Infrastructure.Services.SmsMessaging.Models;

namespace Sygic.Corona.Infrastructure.Services.SmsMessaging
{
    public class MinvSmsMessagingService : ISmsMessagingService
    {
        private readonly HttpClient client;
        readonly XNamespace soapNs = "http://www.w3.org/2003/05/soap-envelope";
        readonly XNamespace smsNs = "http://schema.minv.sk/IP/ESISPZ-SMS/SmsEXT-v1";

        public MinvSmsMessagingService(HttpClient client)
        {
            this.client = client;
        }
        public async Task SendMessageAsync(string messageText, string phoneNumber, CancellationToken cancellationToken)
        {
            XDocument soapRequest = new XDocument(
                new XDeclaration("1.0", "UTF-8", "no"),
                new XElement(soapNs + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "soap", soapNs),
                    new XAttribute(XNamespace.Xmlns + "sms", smsNs),
                    new XElement(soapNs + "Body",
                        new XElement(smsNs + "SendMessage",
                            new XElement(smsNs + "Destination", phoneNumber),
                            new XElement(smsNs + "Message", messageText))
                    )
                )
            );
            await SendRequest(soapRequest, "SendMessage", cancellationToken);
        }

        public async Task PingAsync(CancellationToken cancellationToken)
        {
            XDocument soapRequest = new XDocument(
                new XDeclaration("1.0", "UTF-8", "no"),
                new XElement(soapNs + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "soap", soapNs),
                    new XAttribute(XNamespace.Xmlns + "sms", smsNs),
                    new XElement(soapNs + "Body",
                        new XElement(smsNs + "Ping")
                    )
                )
            );
            await SendRequest(soapRequest, "Ping", cancellationToken);
        }

        private async Task SendRequest(XDocument soapRequest, string ext, CancellationToken cancellationToken)
        {
            using (var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
            })
            {
                request.Content = new StringContent(soapRequest.ToString());
                request.Content.Headers.Clear();

                request.Content.Headers.TryAddWithoutValidation("Content-Type",
                    $"application/soap+xml;action=\"http://schema.minv.sk/IP/ESISPZ-SMS/SmsEXT-v1/ISmsExt/{ext}\"");
                HttpResponseMessage response = await client.SendAsync(request, cancellationToken);


                if (!response.IsSuccessStatusCode)
                {
                    throw new DomainException("Error during sending sms.");
                }

                var resultString = await response.Content.ReadAsStringAsync();
                var result = Deserialize<Envelope>(resultString);

                //api returns HTTP:200 with invalid inputs ...
                var resultStatus = result?.Body?.SendMessageResponse?.SendMessageResult?.ResultStatus;
                if (!string.IsNullOrEmpty(resultStatus) && !string.Equals(resultStatus, "ok", StringComparison.InvariantCultureIgnoreCase))
                {
                    var errMsg = result?.Body.SendMessageResponse?.SendMessageResult?.ErrMsg;
                    throw new DomainException($"Error during sending sms: {resultStatus}: {errMsg}");
                }
            }
        }

        private static T Deserialize<T>(string xmlStr)
        {
            var serializer = new XmlSerializer(typeof(T));
            T result;
            using (TextReader reader = new StringReader(xmlStr))
            {
                result = (T)serializer.Deserialize(reader);
            }
            return result;
        }
    }
}
