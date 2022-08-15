using Bogus;
using EmployeesAPI.DAL;
using EmployeesAPI.Domain.Enums;
using EmployeesAPI.Infrastructure;
using MongoDB.Driver;

namespace EmployeesAPI.Configuration;

public static class DataSeedExtension
{
    public static void DataSeed(this WebApplication? app, string[] args)
    {
        if (!args.Contains("seed-data"))
            return;

        var organizations = app.Services.GetRequiredService<IMongoCollection<OrganizationDataBaseModel>>();
        var members = app.Services.GetRequiredService<IMongoCollection<MemberDataBaseModel>>();
        var users = app.Services.GetRequiredService<IMongoCollection<UserDataBaseModel>>();


        var testOrganization = new Faker<OrganizationDataBaseModel>()
            .RuleFor(o => o.Key, f => f.Random.Guid().ToString())
            .RuleFor(o => o.Name, f => f.Company.CompanyName());
        var organizationCollection = testOrganization.Generate(10);

        var testMember = new Faker<MemberDataBaseModel>()
            .RuleFor(m => m.Key, f => f.Random.Guid().ToString())
            .RuleFor(m => m.Name, f => f.Person.FirstName)
            .RuleFor(m => m.Surname, f => f.Person.LastName)
            .RuleFor(m => m.Age, f => f.Random.Number(18, 99))
            .RuleFor(m => m.OrganizationKey,
                f => organizationCollection[f.Random.Number(0, organizationCollection.Count - 1)].Key);
        var memberCollection = testMember.Generate(20);

        var getPassword = SecurityService.HashString("qwerty");
        var user = new UserDataBaseModel
        {
            Key = Guid.NewGuid().ToString(),
            Email = "defaultuser@gmail.com",
            CreatedAt = DateTime.UtcNow,
            OrganizationId = organizationCollection[0].Key,
            PasswordKey = getPassword.passwordSalt,
            PasswordHash = getPassword.passwordHash,
            Status = EmailStatus.Active
        };

        members.InsertMany(memberCollection);
        organizations.InsertMany(organizationCollection);
        users.InsertOne(user);
    }
}