using EmployeesAPI.Configuration;
using EmployeesAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestSharp.Serializers;
using Xunit;

namespace EmployeeAPI.IntegrationTests;
public class WebAppFixture : IDisposable
{
    public WebApplicationFactory<Program> Application { get; private set; }
    public HttpClient Client { get; private set; }

    public WebAppFixture()
    {
        Application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, confBuilder) =>
            {
                confBuilder.Sources.Clear();
                confBuilder.AddJsonFile("appsettings.Test.json");
            });
            
            builder.ConfigureServices((context, services) =>
            {
                services.Configure<EmployeeMongoDbSettings>(
                    context.Configuration.GetSection("EmployeeMongoDbSettingsTest"));
                services.MongoDbConfiguration(context.Configuration);
            });
        });
        Client = Application.CreateClient();
    }

    public void Dispose()
    {
    }
}