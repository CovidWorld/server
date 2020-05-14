using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Sygic.Corona.Infrastructure.Services.CloudMessaging
{
    public class FirebaseInstanceIdService : IInstanceIdService
    {
        private readonly HttpClient client;

        public FirebaseInstanceIdService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<InstanceInfo> GetInstanceInfoAsync(string pushToken, CancellationToken cancellationToken)
        {
            var response = await client.GetAsync($"info/{pushToken}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            string responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<InstanceInfo>(responseString);
            return result;
        }
    }
}
