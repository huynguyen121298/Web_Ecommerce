using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAndServices.DataModel
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string Name { get; set; }

        public int? Price { get; set; }

        public string Photo { get; set; }

        public string Photo2 { get; set; }

        public string Photo3 { get; set; }

        public string Details { get; set; }

        public int Id_Item { get; set; }

        public string AccountId { get; set; }
    }
}
