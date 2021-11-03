using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using DynamoDbStore.Contract;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DynamoDbStore
{
    public class DynamoDbProvider : IDynamoDbProvider
    {
        readonly IAmazonDynamoDB _dynamoDbClient;
        Table _table;

        public DynamoDbProvider(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        internal void LoadTable(string tableName)
        {
            _table = Table.LoadTable(_dynamoDbClient, tableName);
        }

        public async Task<T> GetItemAsync<T>(string tableName, string primaryKey, CancellationToken cancellationToken = default) where T : class
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

        public async Task<T> GetItemAsync<T>(string tableName, string primaryKey, string sortKey, CancellationToken cancellationToken = default) where T : class
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

        public async Task<T> GetItemAsync<T>(string tableName, string primaryKey, bool consistentRead, List<string> attributesToGet = null, CancellationToken cancellationToken = default) where T:class
        {
            LoadTable(tableName);

            GetItemOperationConfig getItemOperationConfig = new GetItemOperationConfig();
            getItemOperationConfig.AttributesToGet = attributesToGet;
            getItemOperationConfig.ConsistentRead = consistentRead;

            var reponseData = await _table.GetItemAsync(primaryKey, getItemOperationConfig, cancellationToken);
            return JsonConvert.DeserializeObject<T>(reponseData.ToJson());

        }

        public async Task<T> GetItemAsync<T>(string tableName, string primaryKey, string sortKey, bool consistentRead, List<string> attributesToGet = null, CancellationToken cancellationToken = default) where T : class
        {
            LoadTable(tableName);

            GetItemOperationConfig getItemOperationConfig = new GetItemOperationConfig();
            getItemOperationConfig.AttributesToGet = attributesToGet;
            getItemOperationConfig.ConsistentRead = consistentRead;

            var reponseData = await _table.GetItemAsync(primaryKey, sortKey, getItemOperationConfig, cancellationToken);
            return JsonConvert.DeserializeObject<T>(reponseData.ToJson());

        }

        public async Task<bool> UpdateItemAsync<T>(string tableName, string primaryKey, T data, CancellationToken cancellationToken = default) where T : class
        {
            LoadTable(tableName);
            var document = Document.FromJson(JsonConvert.SerializeObject(data));
            var responseRecord = await _table.UpdateItemAsync(document, primaryKey);
            return responseRecord != null;
        }

        public async Task<bool> UpdateItemAsync<T>(string tableName, string primaryKey, string sortKey, T data, CancellationToken cancellationToken = default) where T : class
        {
            LoadTable(tableName);
            var document = Document.FromJson(JsonConvert.SerializeObject(data));
            var responseRecord = await _table.UpdateItemAsync(document, primaryKey, sortKey);
            return responseRecord != null;
        }
    
        public async Task<bool> PutItemAsync<T>(string tableName, T data, CancellationToken cancellationToken = default) where T : class
        {
            LoadTable(tableName);
            var document = Document.FromJson(JsonConvert.SerializeObject(data));
            var reponseData = await _table.PutItemAsync(document);
            return reponseData != null;
        }

        public async Task<bool> DeleteItemAsync<T>(string tableName, string primaryKey, CancellationToken cancellationToken = default) where T : class
        {
            LoadTable(tableName);
            var responseRecord = await _table.DeleteItemAsync(primaryKey, cancellationToken);
            return responseRecord != null;
        }

        public async Task<bool> DeleteItemAsync<T>(string tableName, string primaryKey, string sortKey, CancellationToken cancellationToken = default) where T : class
        {
            LoadTable(tableName);
            var responseRecord = await _table.DeleteItemAsync(primaryKey, sortKey, cancellationToken);
            return responseRecord != null;
        }
    }
}
