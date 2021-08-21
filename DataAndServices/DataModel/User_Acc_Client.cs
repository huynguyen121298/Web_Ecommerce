using MongoDB.Bson.Serialization.Attributes;

namespace DataAndServices.DataModel
{
    public class User_Acc_Client
    {     
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string _id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    
        public string Email { get; set; }

        public string Password { get; set; }

        public int? RoleId { get; set; }
    }
}
