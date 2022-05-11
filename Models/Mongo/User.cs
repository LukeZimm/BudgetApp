using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BudgetApp.Models.Mongo
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        public string Name { get; set; }

        public string AccessToken { get; set; }
    }
}
