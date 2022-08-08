using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EmployeesAPI.DAL;

public class VerificationCodeDataBaseModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Email { get; set; }
    public string Code { get; set; }
    public DateTime GeneratedAt { get; set; }
}