using System.Text;
using EmployeesAPI.Account.AccountsRequests;
using EmployeesAPI.DAL.Repositories;
using EmployeesAPI.Domain;
using EmployeesAPI.Domain.Enums;
using EmployeesAPI.Infrastructure;
using EmployeesAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver.Core.Operations;

namespace EmployeesAPI.Account;

public class UserService
{
    private readonly UserRepository _repository;
    private readonly VerificationCodeService _verificationCodeService;
    private readonly JwtSettings _jwtSettings;

    public UserService(UserRepository repository, IOptions<JwtSettings> options,
        VerificationCodeService verificationCodeService)
    {
        _repository = repository;
        _verificationCodeService = verificationCodeService;
        _jwtSettings = options.Value;
    }

    public async Task<IResult> Register(RegisterUserRequest request)
    {
        var hash = SecurityService.HashString(request.Password);

        if (await _repository.ContainsEmail(request.Email))
            return Results.BadRequest();

        var user = new User(
            request.Email,
            request.OrganizationId
        );
        var res = await _repository.Create(user, hash.passwordHash, hash.passwordSalt);
        if(res == false)
            return Results.BadRequest();
        
        res = await _verificationCodeService.CreateAsync(user.Email);

        return res ? Results.Ok("Confirm email") : Results.BadRequest();
    }

    public async Task<IResult> SignIn(SignInUserRequest request)
    {
        var user = await _repository.FindByEmail(request.Email);
        if (user == null)
            return Results.BadRequest();

        if (!SecurityService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordKey))
            return Results.BadRequest();

        var token = SecurityService.GenerateJwtToken(
            _jwtSettings.Key,
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            user.Key,
            user.OrganizationId);

        return Results.Ok(new JwtResponse(token));
    }

    public async Task<IResult> ConfirmEmail(ConfirmUserEmailRequest request)
    {
        var record = await _verificationCodeService.GetRecordByKey(request.Code);
        if (record == null)
            return Results.BadRequest();

        var user = await _repository.FindByEmail(record.Email);
        if (user == null)
            return Results.BadRequest();

        user.Status = EmailStatus.Active;

        var token = SecurityService.GenerateJwtToken(
            _jwtSettings.Key,
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            user.Key,
            user.OrganizationId);

        return Results.Ok(new JwtResponse(token));
    }
}