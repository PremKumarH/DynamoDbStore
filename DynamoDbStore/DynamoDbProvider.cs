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

        /// <summary>
        /// Initiates the asynchronous execution of the GetItem operation. Amazon.DynamoDBv2.DocumentModel.Table.GetItem
        /// </summary>
        /// <typeparam name="T">The type of element</typeparam>
        /// <param name="tableName">Name of the table</param>
        /// <param name="primaryKey">Primary key element of the document</param>
        /// <param name="cancellationToken">Token which can be used to cancel the task</param>
        /// <returns>The first element that matches the primary key</returns>
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

        /// <summary>
        /// Initiates the asynchronous execution of the GetItem operation. Amazon.DynamoDBv2.DocumentModel.Table.GetItem
        /// </summary>
        /// <typeparam name="T">The type of element</typeparam>
        /// <param name="tableName">Name of the table</param>
        /// <param name="primaryKey">Primary key element of the document</param>
        /// <param name="sortKey">Sort key element of the document</param>
        /// <param name="cancellationToken">Token which can be used to cancel the task</param>
        /// <returns>The first element that matches the Hash key and Range key</returns>
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

        /// <summary>
        /// Initiates the asynchronous execution of the UpdateItem operation. Amazon.DynamoDBv2.DocumentModel.Table.UpdateItem
        /// </summary>
        /// <typeparam name="T">The type of element</typeparam>
        /// <param name="tableName">Name of the table</param>
        /// <param name="primaryKey">Primary key element of the document</param>
        /// <param name="data">Object that needs to be updated in dynamodb</param>
        /// <param name="cancellationToken">Token which can be used to cancel the task</param>
        /// <returns>The updated data</returns>
        public async Task<T> UpdateItemAsync<T>(string tableName, string primaryKey, T data, CancellationToken cancellationToken = default) where T : class
        {
            LoadTable(tableName);
            var document = Document.FromJson(JsonConvert.SerializeObject(data));
            var responseRecord = await _table.UpdateItemAsync(document, primaryKey);
            return JsonConvert.DeserializeObject<T>(responseRecord);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the UpdateItem operation. Amazon.DynamoDBv2.DocumentModel.Table.UpdateItem
        /// </summary>
        /// <typeparam name="T">The type of element</typeparam>
        /// <param name="tableName">Name of the table</param>
        /// <param name="primaryKey">Primary key element of the document</param>
        /// <param name="sortKey">Sort key element of the document</param>
        /// <param name="data">Object that needs to be updated in dynamodb</param>
        /// <param name="cancellationToken">Token which can be used to cancel the task</param>
        /// <returns>The updated data</returns>
        public async Task<T> UpdateItemAsync<T>(string tableName, string primaryKey, string sortKey, T data, CancellationToken cancellationToken = default) where T : class
        {
            LoadTable(tableName);
            var document = Document.FromJson(JsonConvert.SerializeObject(data));
            var responseRecord = await _table.UpdateItemAsync(document, primaryKey, sortKey);
            return JsonConvert.DeserializeObject<T>(responseRecord);
        }
    }
}
