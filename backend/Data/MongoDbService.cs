using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;

namespace backend.Data
{
    public class MongoDbService
    {
        private readonly IMongoCollection<Flyer> _collection;
        private readonly ILogger<MongoDbService> _logger;


        public MongoDbService(IConfiguration configuration, ILogger<MongoDbService> logger)
        {
            //Configuration -> Database connection settings in {'./appsettings.json'}
            var client = new MongoClient(configuration["MongoDbSettings:ConnectionString"]);
            //Create the database
            var database = client.GetDatabase("FlyerDB");
            //Get the collection from the database
            _collection = database.GetCollection<Flyer>("Flyers");
            _logger = logger;
        }

        //Since I'm working with json type validation raw BsonDocument won't work because of cast errors!
        public async Task SaveFlyersAsync(List<Flyer> flyers, string supermarketId)
        {
            foreach(var flyer in flyers)
            {
                flyer.SupermarketID = supermarketId;
            }

            await _collection.InsertManyAsync(flyers);
        }

        public async Task<List<Flyer>> GetFlyersAsync(string supermarketId)
        {
            var filter = Builders<Flyer>.Filter.Eq("SupermarketId", supermarketId);

            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<bool> FlyersExistAsync(string actualDate, string supermarketId)
        {
            var builder = Builders<Flyer>.Filter;
            var filter = builder.Eq(f => f.ActualDate, actualDate) & 
                        builder.Eq(f => f.SupermarketID, supermarketId);

            _logger.LogDebug("The filter values are ActualDate: {AcutalDate}, supermarketId: {supermarketId}", actualDate, supermarketId);


            return await _collection.Find(filter).AnyAsync();
        }
        public async Task<List<Flyer>> GetPageAsnyc(int pageIndex, string supermarketID)
        {
            var builder = Builders<Flyer>.Filter;

            
            var pagefilter = builder.And(
                Builders<Flyer>.Filter.Eq("SupermarketId", supermarketID),
                Builders<Flyer>.Filter.Eq("pageIndex", pageIndex));
            _logger.LogInformation(pagefilter.ToString());

            _logger.LogInformation($"The filter contents are: {pagefilter}");

            return await _collection.Find(pagefilter).ToListAsync();
        }
    }
}
