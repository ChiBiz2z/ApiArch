using EmployeesAPI.Domain;

namespace EmployeeAPI.Tests;

public class DomainMemberTests
{
    [Fact]
    public void MemberNamePropertyTest()
    {
        string name = "Oleg";
        string surname = "Tinkoff";
        Member member = new Member(name, surname, 72, Guid.NewGuid().ToString());
        Assert.NotNull(member);
        Assert.True(!string.IsNullOrEmpty(member.Name));
        Assert.Equal($"{name} {surname}", member.Fullname);
        Assert.True(Guid.TryParse(member.Key, out _));
        Assert.True(Guid.TryParse(member.OrganizationKey, out _));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(15)]
    [InlineData(100)]
    [InlineData(-19)]
    public void MemberAgePropertyTest(int age)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new Member("Oleg", "Tinkoff", age, Guid.NewGuid().ToString()));
    }
}