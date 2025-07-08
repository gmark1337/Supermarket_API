using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

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

        public async Task<bool> FlyersExistAsync(string actualDate)
        {
            var filter = Builders<Flyer>.Filter.Eq("ActualDate", actualDate);
                         
            return await _collection.Find(filter).AnyAsync();
        }
    }
}
