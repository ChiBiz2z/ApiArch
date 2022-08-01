using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace EmployeesAPI.Models
{
    public class Employee
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Surname")]
        public string Surname { get; set; }
        [BsonElement("Age")]
        public int Age { get; set; }
    }
}