using MongoDB.Bson.Serialization.Attributes;

namespace DataAndServices.DataModel
{
    public class Role
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string Details { get; set; }

    }
}
