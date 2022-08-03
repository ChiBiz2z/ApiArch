using EmployeesAPI.DAL.Interfaces;
using EmployeesAPI.Domain;
using MongoDB.Driver;

namespace EmployeesAPI.DAL.Repositories;

public class MemberRepository
{
    private readonly IMongoCollection<MemberDataBaseModel> _memberCollection;

    public MemberRepository(IMongoCollection<MemberDataBaseModel> collection)
    {
        _memberCollection = collection;
    }

    public async Task<bool> Create(Member member)
    {
        var dataBaseModel = new MemberDataBaseModel
        {
            Key = member.Key,
            Name = member.Name,
            Surname = member.Surname,
            Age = member.Age,
            OrganizationKey = member.OrganizationKey
        };
        await _memberCollection.InsertOneAsync(dataBaseModel);
        return true;
    }

    public async Task<bool> Update(Member member)
    {
        var dataBaseModel = new MemberDataBaseModel
        {
            Key = member.Key,
            Age = member.Age,
            Name = member.Name,
            Surname = member.Surname,
            OrganizationKey = member.OrganizationKey
        };

        var update = Builders<MemberDataBaseModel>.Update.Set(
                x => x.Name, dataBaseModel.Name)
            .Set(x => x.Surname, dataBaseModel.Surname)
            .Set(x => x.Age, dataBaseModel.Age)
            .Set(x => x.OrganizationKey, dataBaseModel.OrganizationKey);

        await _memberCollection.UpdateOneAsync(
            o => o.Key == dataBaseModel.Key, update);
        return true;
    }

    public async Task<bool> Delete(string key)
    {
        var delete = Builders<MemberDataBaseModel>.Filter.Eq(
            x => x.Key, key);
        await _memberCollection.DeleteOneAsync(delete);
        return true;
    }


    public async Task<MemberDataBaseModel> GetById(string key) =>
        await _memberCollection.Find(
            m => m.Key == key).FirstOrDefaultAsync();
}