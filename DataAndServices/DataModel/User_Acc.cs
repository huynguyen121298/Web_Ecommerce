using MongoDB.Bson.Serialization.Attributes;

namespace DataAndServices.DataModel
{
    public class User_Acc
    {
        public User_Acc()
        {
            RoleId = 1;
        }
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string _id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        //role 1: user default , role 2 user facebook
        public int? RoleId { get; set; }

        public int? PhoneNumber { get; set; }

        public string City { get; set; }

        public string Address { get; set; }
    }
}
