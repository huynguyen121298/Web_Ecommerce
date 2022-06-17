using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAndServices.DataModel
{
    public class CodeDiscount
    {       
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string Zipcode { get; set; }

        public double Discount { get; set; }

        public string Content { get; set; }
    }
}
