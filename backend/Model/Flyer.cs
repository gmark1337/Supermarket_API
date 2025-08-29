using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Model
{
    //Bson  works the same way as JsonProperty
    //Has to assign which parameter what is because it will drop a cast! error
    public class Flyer
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string flyerID { get; set; }

        [BsonElement("SupermarketId"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string SupermarketID { get; set; }

        [BsonElement("ServiceType"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string ServiceType { get; set; }

        [JsonPropertyName("pageIndex")]
        [BsonElement("pageIndex"), BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
        public int PageIndex { get; set;  }

        [JsonPropertyName("url")]
        [BsonElement("url"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string ImageURL { get; set; }

        [BsonElement("ActualDate"), BsonRepresentation(MongoDB.Bson.BsonType.String)]        
        public string ActualDate {  get; set; }

    }
}
