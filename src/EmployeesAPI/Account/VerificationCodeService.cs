using System.Text;
using EmployeesAPI.Account.AccountsRequests;
using EmployeesAPI.DAL;
using EmployeesAPI.DAL.Repositories;
using EmployeesAPI.Domain;
using EmployeesAPI.Infrastructure;

namespace EmployeesAPI.Account;

public class VerificationCodeService
{
    private readonly VerificationCodeRepository _repository;

    public VerificationCodeService(VerificationCodeRepository repository)
    {
        _repository = repository;
    }


    public async Task<bool> CreateAsync(string email)
    {
        var verificationCode = new VerificationCode(
            email
        );

        var res = await _repository.Create(verificationCode);

        var r = EmailService.SendEmail(email, verificationCode.Code);
        return r.IsSuccessful;
    }

    public async Task<VerificationCodeDataBaseModel> GetRecordByKey(string request)
    {
        return await _repository.GetRecordByCode(request);
    }
}