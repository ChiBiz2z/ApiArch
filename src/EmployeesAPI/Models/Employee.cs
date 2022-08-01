using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace EmployeesAPI.Models
{
    public class Employee
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Name")]
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [BsonElement("Surname")]
        [JsonPropertyName("Surname")]
        public string Surname { get; set; }
        [BsonElement("Age")]
        [JsonPropertyName("Age")]
        public int Age { get; set; }
    }
}