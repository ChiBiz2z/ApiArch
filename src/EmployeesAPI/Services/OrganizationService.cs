using EmployeesAPI.Interfaces;
using EmployeesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EmployeesAPI.Services
{
    public class OrganizationService
    {
        private readonly IMongoCollection<Organization> _organizationCollection;

        public OrganizationService(IEmployeeMongoDbSettings settings)
        {
            var mongoClient = new MongoClient(
                settings.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                settings.DatabaseName);

            _organizationCollection = mongoDatabase.GetCollection<Organization>(
                settings.OrganizationCollectionName);
        }

        public async Task<List<Organization>> GetAsync() =>
            await _organizationCollection.Find(_ => true).ToListAsync();

    }
}
