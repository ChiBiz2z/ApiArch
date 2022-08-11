using System.Net;
using System.Net.Http.Headers;
using System.Text;
using EmployeesAPI.Account;
using EmployeesAPI.Account.AccountsRequests;
using EmployeesAPI.DAL;
using EmployeesAPI.Domain;
using EmployeesAPI.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Xunit;

namespace EmployeeAPI.IntegrationTests;

public class OrganizationServiceTests
{
    [Fact]
    public async Task SignIn_WorksForExistedAccount_Test()
    {
        //Assign
        var application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = application.CreateClient();
        var sr = new SignInUserRequest
        {
            Email = "defikea@gmail.com",
            Password = "qwerty"
        };
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(sr), Encoding.UTF8)
        {
            Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
        };
        //Act
        var response = await client.PostAsync("/account/signin/", content);
        var responseString = await response.Content.ReadAsStringAsync();
        var jwtResponse = System.Text.Json.JsonSerializer.Deserialize<JwtResponse>(responseString);
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(jwtResponse);
        Assert.NotEmpty(jwtResponse.Token);
    }

    [Fact]
    public async Task SignInShouldReturnNotFoundForNotExistedAccount_Test()
    {
        //Assign
        var application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = application.CreateClient();
        var sr = new SignInUserRequest
        {
            Email = "sdfg@gmail.com",
            Password = "asfdg"
        };
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(sr), Encoding.UTF8)
        {
            Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
        };
        //Act
        var response = await client.PostAsync("/account/signin/", content);
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task SignInShouldReturnNotFoundForWrongPassword_Test()
    {
        //Assign
        var application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = application.CreateClient();
        var request = new SignInUserRequest
        {
            Email = "defikeaK@gmail.com",//ALWAYS CHANGE NAME
            Password = "wrongpassword"
        };
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(request), Encoding.UTF8)
        {
            Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
        };
        //Act
        var response = await client.PostAsync("/account/signin/", content);
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CheckSignUpLogic()
    {
        var application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
                services.AddScoped<SendVerificationEmail>(_ => (email, code) => true));
        });
        var client = application.CreateClient();

        var vcodesCollection =
            application.Services.GetRequiredService<IMongoCollection<VerificationCodeDataBaseModel>>();

        var userCollection =
            application.Services.GetRequiredService<IMongoCollection<UserDataBaseModel>>();

        var request = new RegisterUserRequest
        {
            Email = "checknew@gmail.com",
            Password = "qwerty",
            OrganizationId = Guid.NewGuid().ToString()
        };
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(request), Encoding.UTF8)
        {
            Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
        };
        var response = await client.PostAsync("/account/register/", content);

        var user = await userCollection.Find(u => u.Email == request.Email).FirstOrDefaultAsync();
        var verificationCode = await vcodesCollection.Find(c => c.Email == request.Email).FirstOrDefaultAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(verificationCode);
        Assert.NotNull(user);
        Assert.Equal(EmailStatus.PendingVerification, user.Status);
    }
}