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

            var mongoDatabae = mongoClient.GetDatabase(
                employeeOptions.Value.DatabaseName);

            _employeeCollection = mongoDatabae.GetCollection<Employee>(
                employeeOptions.Value.EmployeesCollectionName);
        }
        public async Task<List<Employee>> GetAsync() =>
            await _employeeCollection.Find(_ => true).ToListAsync();

        public async Task<Employee?> GetAsync(int id) =>
            await _employeeCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Employee newBook) =>
            await _employeeCollection.InsertOneAsync(newBook);

        public async Task UpdateAsync(int id, Employee updateEmployee) =>
            await _employeeCollection.ReplaceOneAsync(x => x.Id == id, updateEmployee);

        public async Task RemoveAsync(int id) =>
            await _employeeCollection.DeleteOneAsync(x => x.Id == id);
    }
}
