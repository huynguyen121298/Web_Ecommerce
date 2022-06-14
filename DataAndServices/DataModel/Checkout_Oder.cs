using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DataAndServices.DataModel
{
    public class Checkout_Oder
    {

        public string Id_SanPham { get; set; }

        [StringLength(50)]
        public string TenSP { get; set; }

        public int? SoLuong { get; set; }

        public double? Gia { get; set; }

        public string Size { get; set; }

        public string Color { get; set; }

        public string Photo { get; set; }

        public string ItemId { get; set; }

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string AccountId { get; set; }

    }
}
