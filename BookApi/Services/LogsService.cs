using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApi.Contracts;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookApi.Services
{
    public class LogsService
    {
        private readonly IMongoCollection<Log> _logsCollection;

        public LogsService(
         IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _logsCollection = mongoDatabase.GetCollection<Log>(
            bookStoreDatabaseSettings.Value.LogsCollectionName);
        }

        public async Task CreateLog(Log log) =>
               await _logsCollection.InsertOneAsync(log);
    }
}
