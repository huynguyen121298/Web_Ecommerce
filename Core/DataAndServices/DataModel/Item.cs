using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace DataAndServices.DataModel
{
    public class Item
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public int? Quantity { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }

        public string ProductId { get; set; }
    }
}