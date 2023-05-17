using EmployeesAPI.Domain;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace EmployeesAPI.DAL.Repositories
{
    public class OrganizationRepository
    {
        private readonly IMongoCollection<OrganizationDataBaseModel> _organizationCollection;

        public OrganizationRepository(IMongoCollection<OrganizationDataBaseModel> collection)
        {
            _organizationCollection = collection;
        }

        public async Task<bool> Create(Organization organization)
        {
            var dataBaseModel = new OrganizationDataBaseModel
            {
                Key = organization.Key,
                Name = organization.Name
            };

            await _organizationCollection.InsertOneAsync(dataBaseModel);
            return true;
        }

        public async Task<bool> Update(Organization organization)
        {
            var dataBaseModel = new OrganizationDataBaseModel
            {
                Key = organization.Key,
                Name = organization.Name
            };

            var update = Builders<OrganizationDataBaseModel>.Update.Set(x => x.Name, dataBaseModel.Name);
            await _organizationCollection.UpdateOneAsync(
                o => o.Key == dataBaseModel.Key, update);

            return true;
        }

        public async Task<OrganizationDataBaseModel> GetById(string key) =>
            await _organizationCollection.Find(
                o => o.Key == key).FirstOrDefaultAsync();

        public async Task<List<OrganizationDataBaseModel>> GetAll(string search, int pageNumber, int pageSize)
        {
            Expression<Func<OrganizationDataBaseModel, bool>> filter = string.IsNullOrEmpty(search)
                ? _ => true
                : x => x.Name.ToLower().Contains(search.ToLower());

            var organizationDataBaseModels = await _organizationCollection.AsQueryable()
                .Where(filter)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            return organizationDataBaseModels;
        }

        public async Task<bool> Delete(string key)
        {
            var delete = Builders<OrganizationDataBaseModel>.Filter.Eq(
                x => x.Key, key);
            await _organizationCollection.DeleteOneAsync(delete);
            return true;
        }

        public async Task<bool> ContainsName(string name) =>
            await _organizationCollection.Find(x => x.Name == name).AnyAsync();
    }
}