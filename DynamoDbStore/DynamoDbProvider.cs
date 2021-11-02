using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using DynamoDbStore.Contract;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace DynamoDbStore
{
    public class DynamoDbProvider : IDynamoDbProvider
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;

        public DynamoDbProvider(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        internal Table LoadTable(string tableName)
        {
            return Table.LoadTable(_dynamoDbClient, tableName);
        }

        public async Task<T> GetItemAsync<T>(string tableName, string key) where T : class
        {
            var table = this.LoadTable(tableName);
            var data = await table.GetItemAsync(key);

            if (data == null)
                return null;

            return JsonConvert.DeserializeObject<T>(data.ToJson(), new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

        }

        public async Task<T> PutItemAsync<T>(string tableName, T data) where T : class
        {
            var table = this.LoadTable(tableName);
            var json = JsonConvert.SerializeObject(data);
            var returnData = await table.PutItemAsync(Document.FromJson(json));
            return JsonConvert.DeserializeObject<T>(returnData.ToJson());
        }
        
    }
}
