﻿using EmployeesAPI.DAL;
using EmployeesAPI.DAL.Interfaces;
using EmployeesAPI.DAL.Repositories;
using EmployeesAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EmployeesAPI.Configuration;

public static class ConfigureMongoDb
{
    public static void MongoDbConfiguration(this IServiceCollection services,
        ConfigurationManager manager)
    {
        services.Configure<EmployeeMongoDbSettings>(
            manager.GetSection(nameof(EmployeeMongoDbSettings)));

        services.AddSingleton<IEmployeeMongoDbSettings>(
            provider => provider.GetRequiredService<IOptions<EmployeeMongoDbSettings>>().Value);

        services.AddScoped<OrganizationRepository>();
        services.AddScoped<MemberRepository>();
        services.AddScoped<UserRepository>();
        services.AddScoped<VerificationCodeRepository>();

        var mongoOptions = manager
            .GetSection(nameof(EmployeeMongoDbSettings))
            .Get<EmployeeMongoDbSettings>();

        var mongoClient = new MongoClient(
            mongoOptions.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            mongoOptions.DatabaseName);

        var organizationCollection = mongoDatabase.GetCollection<OrganizationDataBaseModel>(
            mongoOptions.OrganizationCollectionName);

        var memberCollection = mongoDatabase.GetCollection<MemberDataBaseModel>(
            mongoOptions.MembersCollectionName);

        var userCollection = mongoDatabase.GetCollection<UserDataBaseModel>(
            mongoOptions.UsersCollectionName);

        var verificationCodesCollection = mongoDatabase.GetCollection<VerificationCodeDataBaseModel>(
            mongoOptions.VerificationCodesCollectionName);

        services.AddSingleton(organizationCollection);
        services.AddSingleton(memberCollection);
        services.AddSingleton(userCollection);
        services.AddSingleton(verificationCodesCollection);
    }
}