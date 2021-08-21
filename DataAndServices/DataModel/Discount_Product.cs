using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DataAndServices.DataModel
{
    public class Discount_Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string Content { get; set; }

        public Nullable<double> Price_Dis { get; set; }

        public Nullable<System.DateTime> Start { get; set; }

        public Nullable<System.DateTime> End { get; set; }



       
    }
}
