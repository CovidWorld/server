using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sygic.Corona.Infrastructure.Services.CloudMessaging
{
    public class FirebaseCloudMessagingService : ICloudMessagingService
    {
        private readonly HttpClient client;

        public FirebaseCloudMessagingService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<CloudMessagingResponse> SendMessageToDevice(string registrationToken, object data, CancellationToken cancellationToken)
        {
            var jsonObject = JObject.FromObject(data);
            jsonObject.Remove("to");
            jsonObject.Add("to", JToken.FromObject(registrationToken));
            string json = jsonObject.ToString();

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("", content, cancellationToken);

            response.EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<CloudMessagingResponse>(responseString);
            return result;
        }
    }
}
