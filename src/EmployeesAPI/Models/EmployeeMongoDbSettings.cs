namespace EmployeesAPI.Models
{
    public class EmployeeMongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string EmployeesCollectionName { get; set; } = null!;
    }
}
