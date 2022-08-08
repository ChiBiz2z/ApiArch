using System.Text;
using EmployeesAPI.Account;
using EmployeesAPI.Configuration;
using EmployeesAPI.Members;
using EmployeesAPI.Models;
using EmployeesAPI.Organizations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<MemberService>();
builder.Services.AddScoped<OrganizationService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<VerificationCodeService>();

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var basePrefix = "/api";
var organizationPrefix = "/organizations";

app.UseAuthentication();
app.UseAuthorization();

app.MapOrganizationEndpoints($"{basePrefix}{organizationPrefix}");
app.MapMemberEndpoints($"{basePrefix}");
app.MapAccountEndpoints($"{basePrefix}");
app.Run();