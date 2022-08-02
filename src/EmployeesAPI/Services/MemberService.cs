using EmployeesAPI.Interfaces;
using EmployeesAPI.Models;
using MongoDB.Driver;

namespace EmployeesAPI.Services
{
    public class MemberService
    {
        private readonly IMongoCollection<Member> _memberCollection;

        public MemberService(IEmployeeMongoDbSettings settings)
        {
            var mongoClient = new MongoClient(
                settings.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                settings.DatabaseName);

            _memberCollection = mongoDatabase.GetCollection<Member>(
                settings.MembersCollectionName);
        }

        public async Task<List<Member>> GetAsync() =>
            await _memberCollection.Find(_ => true).ToListAsync();

        public async Task<List<Member>> GetByOrganization(string id)
            => await _memberCollection.Find(_ => _.OrganizationId == id).ToListAsync();
    }
}