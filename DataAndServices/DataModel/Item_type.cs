using MongoDB.Bson.Serialization.Attributes;

namespace DataAndServices.DataModel
{
    public class Item_type
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string _id { get; set; }

        public string Type_Product { get; set; }

        public string Status { get; set; }  
    }
}
