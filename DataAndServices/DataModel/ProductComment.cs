using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DataAndServices.DataModel
{
    public class ProductComment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public DateTime DateTimeComment { get; set; }

        public string FullName { get; set; }

        public string Content { get; set; } 

        public string ProductId { get; set; }
    }
}
