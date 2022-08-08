using System.Security.Cryptography;
using EmployeesAPI.Domain;
using MongoDB.Driver;

namespace EmployeesAPI.DAL.Repositories;

public class UserRepository
{
    private readonly IMongoCollection<UserDataBaseModel> _userCollection;

    public UserRepository(IMongoCollection<UserDataBaseModel> collection)
    {
        _userCollection = collection;
    }

    public async Task<bool> Create(User user, string passwordHash, string passwordSalt)
    {
        var dataBaseModel = new UserDataBaseModel
        {
            Key = user.Key,
            Email = user.Email,
            PasswordHash = passwordHash,
            PasswordKey = passwordSalt,
            CreatedAt = user.CreatedAt,
            OrganizationId = user.OrganizationId,
            Status = user.Status
        };

        await _userCollection.InsertOneAsync(dataBaseModel);
        return true;
    }

    public async Task<UserDataBaseModel> FindByEmail(string email) =>
        await _userCollection.Find(x => x.Email == email).FirstOrDefaultAsync();

    public async Task<bool> ContainsEmail(string email) =>
        await _userCollection.Find(x => x.Email == email).AnyAsync();
}