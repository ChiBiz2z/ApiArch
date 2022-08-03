using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace EmployeesAPI.DAL
{
    public class OrganizationDataBaseModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Key { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }
    }

}