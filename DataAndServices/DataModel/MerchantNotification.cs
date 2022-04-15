using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAndServices.DataModel
{
    public class MerchantNotification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string AccountId { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }
    }
}
