using EmployeesAPI.Account.AccountsRequests;

namespace EmployeesAPI.Account;

public static class AccountEndPoints
{
    public static WebApplication? MapAccountEndpoints(this WebApplication? app, string prefix)
    {
        if (app == null)
            return app;

        app.MapPost("/account/register/",
            (UserService service, RegisterUserRequest request) => 
                service.Register(request)).WithTags("Account");

        app.MapPost("/account/login/",
            (UserService service, LoginUserRequest request) => 
                Results.Ok()).WithTags("Account");

        return app;
    }
}