using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace DynamoDbStore.Contract
{
    interface IDynamoDbProvider
    {
        Task<T> GetItemAsync<T>(string tableName, string primaryKey, CancellationToken cancellationToken = default) where T : class;
        Task<T> GetItemAsync<T>(string tableName, string primaryKey, string sortKey, CancellationToken cancellationToken = default) where T : class;
        Task<T> GetItemAsync<T>(string tableName, string primaryKey, bool consistentRead, List<string> attributesToGet = null, CancellationToken cancellationToken = default) where T : class;
        Task<T> GetItemAsync<T>(string tableName, string primaryKey, string sortKey, bool consistentRead, List<string> attributesToGet = null, CancellationToken cancellationToken = default) where T : class;
        Task<bool> UpdateItemAsync<T>(string tableName, string primaryKey, T data, CancellationToken cancellationToken = default) where T : class;
        Task<bool> UpdateItemAsync<T>(string tableName, string primaryKey, string sortKey, T data, CancellationToken cancellationToken = default) where T : class;
        Task<bool> PutItemAsync<T>(string tableName, T data, CancellationToken cancellationToken = default) where T : class;
        Task<bool> DeleteItemAsync<T>(string tableName, string primaryKey, CancellationToken cancellationToken = default) where T : class;
        Task<bool> DeleteItemAsync<T>(string tableName, string primaryKey, string sortKey, CancellationToken cancellationToken = default) where T : class
    }
}
