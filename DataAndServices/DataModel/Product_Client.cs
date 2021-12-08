using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace DataAndServices.DataModel
{
    public class Product_Client
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string _id { get; set; }

        public string Name { get; set; }

        public int? Price { get; set; }

        public string Photo { get; set; }

        public string Photo2 { get; set; }

        public string Photo3 { get; set; }

        public string Details { get; set; }

        public int Id_Item { get; set; }
    }
}
