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

        public async Task<IResult> GetById(string id)
        {
            var member = await _memberCollection.Find(
                o => o.Id == id).FirstOrDefaultAsync();

            return member != null ? Results.Ok(member) : Results.NotFound();
        }

        public async Task CreateAsync(Member newMember)
        {
            await _memberCollection.InsertOneAsync(newMember);
        }


        public async Task RemoveAsync(string id) =>
            await _memberCollection.DeleteOneAsync(m => m.Id == id);

        public async Task UpdateAsync(string id, Member updMember) =>
            await _memberCollection.ReplaceOneAsync(x => x.Id == id, updMember);
    }
}