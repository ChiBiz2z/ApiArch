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

        //TODO СДЕЛАТЬ ПРОВЕРКУ НА NULL И ВОЗВРАЩАТЬ BADREQUEST'ы т.п
        public async Task<Organization> GetById(string id) =>
            await _organizationCollection.Find(o => o.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Organization newOrganization) =>
            await _organizationCollection.InsertOneAsync(newOrganization);

        public async Task RemoveAsync(string id) =>
            await _organizationCollection.DeleteOneAsync(o => o.Id == id);

        public async Task UpdateAsync(string id, Organization newOrganization) =>
            await _organizationCollection.ReplaceOneAsync(o => o.Id == id, newOrganization);
    }
}
