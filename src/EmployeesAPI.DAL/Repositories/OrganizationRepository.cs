using EmployeesAPI.Domain;
using MongoDB.Driver;

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