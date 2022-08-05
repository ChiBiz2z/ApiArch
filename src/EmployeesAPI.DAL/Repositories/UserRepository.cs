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

    public async Task<bool> Create(User user)
    {
        var dataBaseModel = new UserDataBaseModel
        {
            Key = user.Key,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            PasswordKey = user.PasswordKey,
            CreatedAt = user.CreatedAt,
            OrganizationId = user.OrganizationId
        };

        await _userCollection.InsertOneAsync(dataBaseModel);
        return true;
    }
}