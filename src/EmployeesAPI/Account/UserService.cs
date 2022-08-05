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
        //TODO проверка на существующую почту
        var user = new User(
            request.Email,
            hash.passwordHash,
            "Organizationid",
            hash.passwordSalt
        );

        var token = SecurityService.GenerateJwtToken(
            _jwtSettings.Key,
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            user.Key, 
            user.OrganizationId);
        
        var res = await _repository.Create(user);
        
        return res ? Results.Ok(new JwtResponse(token)) : Results.BadRequest();
    }
}