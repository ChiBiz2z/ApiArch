using System.Net;
using Bogus;
using System.Net.Http.Headers;
using System.Text;
using Bogus.DataSets;
using EmployeesAPI.DAL;
using EmployeesAPI.DAL.Interfaces;
using EmployeesAPI.Models;
using EmployeesAPI.Organizations;
using EmployeesAPI.Organizations.OrganizationRequests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Xunit;

namespace EmployeeAPI.IntegrationTests;

public class OrganizationTests : IClassFixture<DataBaseFixture>
{
    private readonly DataBaseFixture _fixture;

    public OrganizationTests(DataBaseFixture fixture)
    {
        _fixture = fixture;
    }


    //?
    [Fact]
    public async Task OrganizationShouldBeFoundById_Test()
    {
        var application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var oldDbConfig = services.SingleOrDefault(s =>
                    s.ServiceType == typeof(IEmployeeMongoDbSettings));

                if (oldDbConfig != null)
                    services.Remove(oldDbConfig);
                //TODO ???????????
                services.AddSingleton<IEmployeeMongoDbSettings>(
                    provider => provider.GetRequiredService<IOptions<EmployeeMongoDbSettings>>().Value);
            });
        });

        var client = application.CreateClient();
        
        var request = new GetOrganizationRequest
        {
            Key = "61825ee4-4def-4c61-a2dd-c0016bec895e"
        };

        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(request), Encoding.UTF8)
        {
            Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
        };

        //var response = await client.GetAsync("/organization/", content); //?

        //Assert
    }

    //Ok
    [Fact]
    public async Task Organization_Should_Be_Created_Test()
    {
        //TODO Should be normal DB configuration
        var application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = application.CreateClient();

        var userCollection =
            application.Services.GetRequiredService<IMongoCollection<UserDataBaseModel>>();

        var faker = new Faker();
        var request = new CreateOrganizationRequest
        {
            Name = faker.Company.CompanyName(),
            DefaultUserEmail = faker.Internet.Email().ToLower(),
            DefaultUserPassword = "qwerty"
        };

        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(request), Encoding.UTF8)
        {
            Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
        };

        var response = await client.PostAsync("/organizations/", content);
        var user = await userCollection.Find(
            u => u.Email == request.DefaultUserEmail).FirstOrDefaultAsync();

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(user);
    }

    //?
    [Fact]
    public async Task Organization_Should_Not_Be_Created_Test()
    {
        //Assign
        //TODO Should be normal DB configuration
        var faker = new Faker();
        var application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = application.CreateClient();
        var organizationCollection =
            application.Services.GetRequiredService<IMongoCollection<OrganizationDataBaseModel>>();

        var existingOrganization =
            await organizationCollection.Find(_ => true).FirstOrDefaultAsync();

        //Нужна ли эта часть? Вроде как запись 99% должна быть
        var preRequest = new CreateOrganizationRequest
        {
            Name = faker.Name.FirstName(Name.Gender.Male),
            DefaultUserEmail = faker.Internet.Email().ToLower(),
            DefaultUserPassword = "qwerty"
        };

        var converter = (OrganizationService service) => service.CreateAsync(preRequest);

        var request = new CreateOrganizationRequest
        {
            Name = existingOrganization.Name,
            DefaultUserEmail = faker.Internet.Email().ToLower(),
            DefaultUserPassword = "qwerty"
        };

        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(request), Encoding.UTF8)
        {
            Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
        };
        //Act
        var response = await client.PostAsync("/organization/", content);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Organization_Should_Be_Updated_Test()
    {
        //TODO Should be normal DB configuration
        var faker = new Faker();
        var application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = application.CreateClient();

        var request = new UpdateOrganizationRequest
        {
            Key = "61825ee4-4def-4c61-a2dd-c0016bec895e",
            Name = faker.Company.CompanyName(),
        };
        
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(request), Encoding.UTF8)
        {
            Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
        };
        
        var organizationCollection =
            application.Services.GetRequiredService<IMongoCollection<OrganizationDataBaseModel>>();
        
        //Act
        var response = await client.PutAsync("/organization/", content);
        
        var organizationUpd = await organizationCollection
            .Find(o => o.Key == request.Key).FirstOrDefaultAsync();
        
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(request.Name, organizationUpd.Name);
    }
}