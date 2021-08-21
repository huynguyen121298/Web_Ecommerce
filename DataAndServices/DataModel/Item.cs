using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAndServices.DataModel
{
    public class Item
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public int? Quantity { get; set; }

    }
}
