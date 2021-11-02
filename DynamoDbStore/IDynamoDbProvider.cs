using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDbStore.Contract
{
    interface IDynamoDbProvider
    {
        Task<T> GetItemAsync<T>(string tableName, string key) where T : class;
        Task<T> PutItemAsync<T>(string tableName, T data) where T : class;
        Task<T> UpdateItemAsync<T>(string tableName, T data) where T : class;
        Task<T> UpdateItemAsync<T>(string tableName, string key, T data) where T : class;
    }
}
