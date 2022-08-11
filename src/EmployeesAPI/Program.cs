using System.Text;
using EmployeesAPI.Account;
using EmployeesAPI.Configuration;
using EmployeesAPI.Domain;
using EmployeesAPI.Errors;
using EmployeesAPI.Infrastructure;
using EmployeesAPI.Members;
using EmployeesAPI.Models;
using EmployeesAPI.Organizations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.Seq("http://localhost:5341")
    .CreateBootstrapLogger();

builder.Host.UseSerilog();

builder.Services.AddScoped<MemberService>();
builder.Services.AddScoped<OrganizationService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<VerificationCodeService>();
builder.Services.AddScoped<SendVerificationEmail>(_ =>
    (email, code) => EmailService.SendEmail(email, code).IsSuccessful
);

builder.Services.ConfigureSwagger()
    .MongoDbConfiguration(builder.Configuration);

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection(nameof(JwtSettings)));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "Default",
        policy => { policy.RequireClaim("organizationId"); });
});

try
{
    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseExceptionHandler(e
        => e.CustomErrors(app.Environment));

    app.UseHttpsRedirection();
    app.UseMiddlewareLogging();

    var basePrefix = "/api";
    var organizationPrefix = "/organizations";

    app.UseAuthentication();
    app.UseAuthorization();

    app.DataSeed(args);
    app.MapOrganizationEndpoints($"{basePrefix}{organizationPrefix}");
    app.MapMemberEndpoints($"{basePrefix}");
    app.MapAccountEndpoints($"{basePrefix}");
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program
{
}