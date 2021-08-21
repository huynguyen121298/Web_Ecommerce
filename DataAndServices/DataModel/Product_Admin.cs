using MongoDB.Bson.Serialization.Attributes;

namespace DataAndServices.DataModel
{
    public class Product_Admin
    {
        public Product_Admin()
        {
            Photo = "~/images_product/ap.jpg";
        }
        
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string _id { get; set; }

        public string Name { get; set; }

        public int? Price { get; set; }

        public string Photo { get; set; }

        public string Details { get; set; }

        public int Id_Item { get; set; }
    }
}
