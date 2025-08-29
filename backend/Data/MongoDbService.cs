using backend.Model;
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
        private readonly IMongoCollection<FlyerPDF> _pdfCollection;
        private readonly IMongoCollection<Feedback> _feedbackCollection;
        private readonly ILogger<MongoDbService> _logger;


        public MongoDbService(IConfiguration configuration, ILogger<MongoDbService> logger)
        {
            //Configuration -> Database connection settings in {'./appsettings.json'}
            var client = new MongoClient(configuration["MongoDbSettings:ConnectionString"]);

            var flyerCollectionName = configuration["MongoDbSettings:FlyerCollection"];
            var flyerPDFCollectionName = configuration["MongoDbSettings:FlyerPDFCollection"];
            var feedbackCollectionName = configuration["MongoDbSettings:FeedbackCollection"];
            var databaseName = configuration["MongoDbSettings:Database"];

            //Create the database
            var database = client.GetDatabase(databaseName); 

            //Get the collection from the database
            _collection = database.GetCollection<Flyer>(flyerCollectionName);
            _pdfCollection = database.GetCollection<FlyerPDF>(flyerPDFCollectionName);
            _feedbackCollection = database.GetCollection<Feedback>(feedbackCollectionName);

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

        //PDF
        public async Task SaveFlyerPdfAsync(FlyerPDF flyerObject, string supermarketId)
        {
            flyerObject.SupermarketId = supermarketId;

            await _pdfCollection.InsertOneAsync(flyerObject);
        }
        public async Task<FlyerPDF> GetFlyerPdfbyActualDateASync(string acutalDate)
        {
            var filter = Builders<FlyerPDF>.Filter.Eq("ActualDate", acutalDate);

            _logger.LogInformation($"The filter's data are: {filter}");

            return await _pdfCollection.Find(filter).FirstOrDefaultAsync();
        }
        

        public async Task<List<Flyer>> GetFlyersAsync(string supermarketId)
        {
            var filter = Builders<Flyer>.Filter.Eq("SupermarketId", supermarketId);

            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<List<FlyerPDF>> GetFlyerPdfAsync(string supermarketId)
        {
            var filter = Builders<FlyerPDF>.Filter.Eq("SupermarketId", supermarketId);

            return await _pdfCollection.Find(filter).ToListAsync();
        }

        public async Task<List<Flyer>> GetFlyersByActualDateAsync(string acutalDate)
        {
            var filter = Builders<Flyer>.Filter.Eq("ActualDate", acutalDate);

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

        public async Task<bool> FlyerPdfExistAsync(string supermarketId, string actualDate)
        {
            var builder = Builders<FlyerPDF>.Filter;

            var pagefilter = builder.And(
                builder.Eq("SupermarketId", supermarketId),
                builder.Eq("ActualDate", actualDate)
                );
            return await _pdfCollection.Find(pagefilter).AnyAsync();
        }


        public async Task SaveFeedbackAsync(Feedback feedback)
        {
            _logger.LogInformation($"Inserting... \n  {feedback.FeedbackId}");
            await _feedbackCollection.InsertOneAsync(feedback);
        }

        public async Task<Feedback> GetFeedbackAsync(string id)
        {
            var filter = Builders<Feedback>.Filter.Eq("FeedbackId", id);


            return await _feedbackCollection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
