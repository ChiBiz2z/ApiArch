using System.Text;
using EmployeesAPI.Account.AccountsRequests;
using EmployeesAPI.DAL.Repositories;
using EmployeesAPI.Domain;
using EmployeesAPI.Infrastructure;
using EmployeesAPI.Models;
using Microsoft.Extensions.Options;

namespace EmployeesAPI.Account;

public class UserService
{
    private readonly UserRepository _repository;
    private readonly JwtSettings _jwtSettings;

    public UserService(UserRepository repository, IOptions<JwtSettings> options)
    {
        _repository = repository;
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

        var token = SecurityService.GenerateJwtToken(
            _jwtSettings.Key,
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            user.Key,
            user.OrganizationId);

        var res = await _repository.Create(user, hash.passwordHash, hash.passwordSalt);

        return res ? Results.Ok(new JwtResponse(token)) : Results.BadRequest();
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
}