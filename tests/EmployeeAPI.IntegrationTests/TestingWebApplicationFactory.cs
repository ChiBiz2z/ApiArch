using EmployeesAPI.DAL.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EmployeeAPI.IntegrationTests;

public class TestingWebApplicationFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IEmployeeMongoDbSettings));

            if (descriptor != null)
                services.Remove(descriptor);

            var app = builder.Build();
            
            services.MongoDbConfigurationTests();
        });
    }
}