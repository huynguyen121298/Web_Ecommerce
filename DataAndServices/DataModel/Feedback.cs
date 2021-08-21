using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAndServices.DataModel
{
    public class Feedback
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string SDT { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Content { get; set; }

        public string Details { get; set; }
    }
}
