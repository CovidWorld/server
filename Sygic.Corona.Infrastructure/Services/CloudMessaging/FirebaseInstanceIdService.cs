using Newtonsoft.Json;
using Sygic.Corona.Domain;
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

            var result = JsonConvert.DeserializeObject<InstanceInfoResponse>(responseString);
            var info = new InstanceInfo
            {
                Application = result.Application,
                ApplicationVersion = result.ApplicationVersion,
                Scope = result.Scope,
                Platform = result.Platform switch
                {
                    "ANDROID" => Platform.Android,
                    "IOS" => Platform.Ios,
                    _ => Platform.Undefined
                }
            };
            return info;
        }
    }

    public class InstanceInfoResponse
    {
        public string ApplicationVersion { get; set; }
        public string Application { get; set; }
        public string Scope { get; set; }
        public string AuthorizedEntity { get; set; }
        public string Platform { get; set; }
    }
}
