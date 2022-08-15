using EmployeesAPI.DAL;
using EmployeesAPI.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace EmployeeAPI.IntegrationTests;

public class DataBaseFixture : IDisposable
{
    public MongoClient Client { get; }
    public IMongoDatabase Database { get; }

    public DataBaseFixture()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var mongoOptions = config.GetSection("EmployeeMongoDbSettingsTest").Get<EmployeeMongoDbSettings>();

        Client = new MongoClient(mongoOptions.ConnectionString);
        Database = Client.GetDatabase("MinimalApiDBTest");
        
        Database.CreateCollection("Organizations");
        Database.GetCollection<OrganizationDataBaseModel>("Organizations").InsertMany(
            new List<OrganizationDataBaseModel>
            {
                new()
                {
                    Key = "61825ee4-4def-4c61-a2dd-c0016bec895e",
                    Name = "Default Organization #1"
                },
                new()
                {
                    Key = Guid.NewGuid().ToString(),
                    Name = "Default Organization #2"
                }
            });

        Database.CreateCollection("Members");
        Database.GetCollection<MemberDataBaseModel>("Members").InsertOne(
            new MemberDataBaseModel
            {
                Key = Guid.NewGuid().ToString(),
                Name = "Default Member",
                Surname = "#1",
                Age = 22,
                OrganizationKey = "61825ee4-4def-4c61-a2dd-c0016bec895e"
            });
        
        Database.CreateCollection("Users");
        Database.CreateCollection("VerificationCodes");
        
    }

    public void Dispose()
    {
        Client.DropDatabase("MinimalApiDBTest");
    }
}