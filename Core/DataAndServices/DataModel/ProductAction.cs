using MongoDB.Bson.Serialization.Attributes;

namespace DataAndServices.DataModel
{
    public class ProductAction
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string _id { get; set; }

        public string UserId { get; set; }

        public string ProductId { get; set; }


        /// <summary>
        /// 1 Bought / 2. Favorite
        /// </summary>
        public int Status { get; set; }

    }
}
