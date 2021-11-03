using System.Threading;
using System.Threading.Tasks;


namespace DynamoDbStore.Contract
{
    interface IDynamoDbProvider
    {
        Task<T> GetItemAsync<T>(string tableName, string primaryKey, CancellationToken cancellationToken = default) where T : class;
        Task<T> GetItemAsync<T>(string tableName, string primaryKey, string sortKey, CancellationToken cancellationToken = default) where T : class;
        Task<T> UpdateItemAsync<T>(string tableName, string primaryKey, T data, CancellationToken cancellationToken = default) where T : class;
        Task<T> UpdateItemAsync<T>(string tableName, string primaryKey, string sortKey, T data, CancellationToken cancellationToken = default) where T : class;
    }
}
