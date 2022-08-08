using EmployeesAPI.Domain;
using MongoDB.Driver;

namespace EmployeesAPI.DAL.Repositories;

public class VerificationCodeRepository
{
    private readonly IMongoCollection<VerificationCodeDataBaseModel> _codesCollection;

    public VerificationCodeRepository(IMongoCollection<VerificationCodeDataBaseModel> collection)
    {
        _codesCollection = collection;
    }

    public async Task<bool> Create(VerificationCode vcode)
    {
        var dataBaseModel = new VerificationCodeDataBaseModel
        {
            Code = vcode.Code,
            Email = vcode.Email,
            GeneratedAt = vcode.GeneratedAt
        };

        await _codesCollection.InsertOneAsync(dataBaseModel);
        return true;
    }

    public async Task<VerificationCodeDataBaseModel> GetRecordByCode(string code) =>
        await _codesCollection.Find(x => x.Code == code).FirstOrDefaultAsync();
}