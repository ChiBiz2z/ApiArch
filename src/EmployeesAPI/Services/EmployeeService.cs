using EmployeesAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EmployeesAPI.Services
{
    public class EmployeeService
    {
        private readonly IMongoCollection<Employee> _employeeCollection;

        public EmployeeService(IOptions<EmployeeMongoDbSettings> employeeOptions)
        {
            var mongoClient = new MongoClient(
                employeeOptions.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                employeeOptions.Value.DatabaseName);

            _employeeCollection = mongoDatabase.GetCollection<Employee>(
                employeeOptions.Value.EmployeesCollectionName);
        }

        public async Task<List<Employee>> GetAsync() =>
            await _employeeCollection.Find(_ => true).ToListAsync();

        public async Task<Employee?> GetAsync(string id) =>
            await _employeeCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Employee newEmployee) =>
            await _employeeCollection.InsertOneAsync(newEmployee);

        public async Task UpdateAsync(string id, Employee updateEmployee) =>
            await _employeeCollection.ReplaceOneAsync(x => x.Id == id, updateEmployee);

        public async Task RemoveAsync(string id) =>
            await _employeeCollection.DeleteOneAsync(x => x.Id == id);
    }
}