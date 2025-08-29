using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace backend.Model
{
    public class FlyerPDF
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string PdfID { get; set; }

        [BsonElement("SupermarketId"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string SupermarketId { get; set; }

        [JsonPropertyName("ActualDate")]
        [BsonElement("ActualDate"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string ActualDate { get; set; }

        [JsonPropertyName("FirstPageURL")]
        [BsonElement("FirstPageURL"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string FirstPageURL { get; set; }

        [JsonPropertyName("URL")]
        [BsonElement("URL"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string PdfURL { get; set; }

    }
}
