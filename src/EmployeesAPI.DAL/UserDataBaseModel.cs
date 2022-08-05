using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EmployeesAPI.DAL;

public class UserDataBaseModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Key { get; set;}
    public string Email { get; set;}
    public string PasswordHash { get; set;}
    public string PasswordKey { get; set;}
    public DateTime CreatedAt { get; set;}
    public string OrganizationId { get; set;}
}