using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDbStore.Contract
{
    interface IDynamoDbReadProvider
    {
        Task<T> GetItemAsync<T>(string tableName, string primaryKey) where T : class;
        Task<T> GetItemAsync<T>(string tableName, string primaryKey, string sortKey) where T : class;
    }

    interface IDynamoDbUpsertProvider
    {
        Task<T> PutItemAsync<T>(string tableName, T data) where T : class;
        Task<T> UpdateItemAsync<T>(string tableName, T data) where T : class;
        Task<T> UpdateItemAsync<T>(string tableName, string primaryKey, T data) where T : class;
        Task<T> UpdateItemAsync<T>(string tableName, string primaryKey, string sortKery, T data) where T : class;
    }
    interface IDynamoDbDeleteProvider
    {

    }
}
