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

        public async Task<IResult> GetById(string id)
        {
            var organization = await _organizationCollection.Find(
                o => o.Id == id).FirstOrDefaultAsync();

            return organization != null ? Results.Ok(organization) : Results.NotFound();
        }




        public async Task<IResult> CreateAsync(Organization newOrganization)
        {
            if (string.IsNullOrEmpty(newOrganization.Name))
                return Results.BadRequest();

            await _organizationCollection.InsertOneAsync(newOrganization);
            return Results.Ok();
        }


        public async Task RemoveAsync(string id) =>
            await _organizationCollection.DeleteOneAsync(o => o.Id == id);

        public async Task UpdateAsync(string id, Organization newOrganization) =>
            await _organizationCollection.ReplaceOneAsync(o => o.Id == id, newOrganization);
    }
}
