using EmployeesAPI.Account.AccountsRequests;

namespace EmployeesAPI.Account;

public static class AccountEndPoints
{
    public static WebApplication? MapAccountEndpoints(this WebApplication? app, string prefix)
    {
        if (app == null)
            return app;

        app.MapPost("/account/register/",
            async (UserService service, RegisterUserRequest request) =>
                await service.Register(request)).WithTags("Account");

        app.MapPost("/account/signin/",
            async (UserService service, SignInUserRequest request) =>
                await service.SignIn(request)).WithTags("Account");

        return app;
    }
}