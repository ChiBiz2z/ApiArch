using Xunit;

namespace EmployeeAPI.IntegrationTests;

[CollectionDefinition("AppFixture")]
public class WebAppCollection : ICollectionFixture<WebAppFixture>
{
    
}