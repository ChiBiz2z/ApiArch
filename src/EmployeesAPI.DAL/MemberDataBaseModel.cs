using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EmployeesAPI.DAL;

public class MemberDataBaseModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Key { get; set; }
    [BsonElement("Name")]
    public string Name { get; set; }
    [BsonElement("Surname")]
    public string Surname { get; set; }
    [BsonElement("Age")]
    public int Age { get; set; }
    [BsonElement("OrganizationKey")]
    public string OrganizationKey { get; set; }
}