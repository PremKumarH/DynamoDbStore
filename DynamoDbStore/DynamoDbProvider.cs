using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using DynamoDbStore.Contract;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace DynamoDbStore
{
    public class DynamoDbProvider : IDynamoDbReadProvider, IDynamoDbUpsertProvider, IDynamoDbDeleteProvider
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        Table _table;

        public DynamoDbProvider(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        internal void LoadTable(string tableName)
        {
            _table = Table.LoadTable(_dynamoDbClient, tableName);
        }

        public async Task<T> GetItemAsync<T>(string tableName, string primaryKey) where T : class
        {
            LoadTable(tableName);
            var data = await _table.GetItemAsync(primaryKey);

            if (data == null)
                return null;

            return JsonConvert.DeserializeObject<T>(data.ToJson(), new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        public async Task<T> GetItemAsync<T>(string tableName, string primaryKey, string sortKey) where T : class
        {
            LoadTable(tableName);
            var data = await _table.GetItemAsync(primaryKey, sortKey);

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
            LoadTable(tableName);
            var json = JsonConvert.SerializeObject(data);
            var returnData = await _table.PutItemAsync(Document.FromJson(json));
            return JsonConvert.DeserializeObject<T>(returnData.ToJson());
        }
        
        public async Task<T> UpdateItemAsync<T>(string tableName, T data) where T : class
        {
            LoadTable(tableName);
            var documnet = Document.FromJson(JsonConvert.SerializeObject(data));
            var responseRecord = await _table.UpdateItemAsync(documnet);
            return JsonConvert.DeserializeObject<T>(responseRecord.ToJson());
        }

        public async Task<T> UpdateItemAsync<T>(string tableName, string primaryKey, T data) where T : class
        {
            LoadTable(tableName);
            var document = Document.FromJson(JsonConvert.SerializeObject(data));
            var responseRecord = await _table.UpdateItemAsync(document, primaryKey);
            return JsonConvert.DeserializeObject<T>(responseRecord);
        }

        public async Task<T> UpdateItemAsync<T>(string tableName, string primaryKey, string sortKery, T data) where T : class
        {
            LoadTable(tableName);
            var document = Document.FromJson(JsonConvert.SerializeObject(data));
            var responseRecord = await _table.UpdateItemAsync(document, primaryKey, sortKery);
            return JsonConvert.DeserializeObject<T>(responseRecord);
        }
    }
}
