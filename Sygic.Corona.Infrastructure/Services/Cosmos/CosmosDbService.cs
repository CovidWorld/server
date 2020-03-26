using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace Sygic.Corona.Infrastructure.Services.Cosmos
{
    public partial class CosmosDbService : ICosmosDbService
    {
        private readonly Container container;

        public CosmosDbService(CosmosClient dbClient, string databaseName, string containerName)
        {
            container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task<IEnumerable<ProfileModel>> GetProfilesByPhoneNumberSearchTermAsync(string searchTerm, int limit, CancellationToken cancellationToken)
        {
            
            var queryString =
                "SELECT TOP @limit c.ProfileId, c.DeviceId, c.PhoneNumber, c.LastPositionReportTime, c.IsInQuarantine FROM c WHERE c.Discriminator = 'Profile' AND CONTAINS(c.PhoneNumber, @searchTerm)";
            var queryDefinition = new QueryDefinition(queryString)
                .WithParameter("@searchTerm", searchTerm)
                .WithParameter("@limit", limit);

            var query = container.GetItemQueryIterator<ProfileModel>(queryDefinition);
            var results = new List<ProfileModel>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync(cancellationToken);

                results.AddRange(response.ToList());
            }

            return results;
        }
    }
}
