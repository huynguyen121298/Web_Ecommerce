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

        public List<string> Color { get; set; } 

        public List<string> Size { get; set; }


    }
}
