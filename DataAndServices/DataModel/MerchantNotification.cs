using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

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

        public string CheckoutId { get; set; }  

        public int Status { get; set; }

        public DateTime? DateTime { get; set; } 
    }
}
