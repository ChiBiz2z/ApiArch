using System.Runtime.InteropServices;
using EmployeesAPI.Domain;

namespace EmployeeAPI.Tests;

public class DomainOrganizationTests
{
    [Fact]
    public void TestOrganizationNameProperty()
    {
        string name = "Tinkoff";
        Organization organization = new Organization(name);
        Assert.NotNull(organization);
        Assert.True(!string.IsNullOrEmpty(organization.Name));
        Assert.True(Guid.TryParse(organization.Key, out _));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void OrganizationWithInvalidNameShouldntBeCreated(string name)
    {
        Assert.Throws<ArgumentNullException>(() => new Organization(name));
    }
}