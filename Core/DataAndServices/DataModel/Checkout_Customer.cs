using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DataAndServices.DataModel
{
    public class Checkout_Customer
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string _Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int SDT { get; set; }

        public string DiaChi { get; set; }

        public string City { get; set; }

        public string Zipcode { get; set; }

        public DateTime? NgayTao { get; set; }

        public string GiamGia { get; set; }

        public double? TongTien { get; set; }

        public string TrangThai { get; set; }
    }
}
