using Azure;
using Azure.Data.Tables;
using NatheemScott_ST10109685_CLDVPOEPart1.Models;
using System.Threading.Tasks;

namespace NatheemScott_ST10109685_CLDVPOEPart1.Services
{
    public class TableService
    {
        private readonly TableClient _tableClient;

        public TableService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureStorage:ConnectionString"];
            var serviceClient = new TableServiceClient(connectionString);
            _tableClient = serviceClient.GetTableClient("CustomerProfiles");
            _tableClient.CreateIfNotExists();
        }

        public async Task AddEntityAsync(CustomerProf profile)
        {
            await _tableClient.AddEntityAsync(profile);
        }
    }
}

