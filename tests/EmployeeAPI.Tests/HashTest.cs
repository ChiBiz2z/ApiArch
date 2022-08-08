using EmployeesAPI.Infrastructure;

namespace EmployeeAPI.Tests;

public class HashTest
{
    [Fact]
    public void CheckUserPasswordHash()
    {
        string password = "qwerty123";
        var originalHash = SecurityService.HashString(password);

        Assert.True(SecurityService.VerifyPasswordHash(password, originalHash.passwordHash, originalHash.passwordSalt));
    }
}