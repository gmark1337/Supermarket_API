using MongoDB.Bson.Serialization.Attributes;

namespace backend.Model
{
    public class Feedback
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string FeedbackId { get; set; }

        [BsonElement("Username"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Username { get; set; }

        [BsonElement("FeedbackText"),BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string FeedBackText { get; set; }

        [BsonElement("DateOfSubmit"), BsonRepresentation(MongoDB.Bson.BsonType.DateTime)]

        public DateTime DateOfSubmit { get; set; } = DateTime.Today;

    }
}
