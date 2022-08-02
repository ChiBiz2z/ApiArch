using EmployeesAPI.Interfaces;
using EmployeesAPI.Models;
using EmployeesAPI.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<EmployeeMongoDbSettings>(
    builder.Configuration.GetSection(nameof(EmployeeMongoDbSettings)));

builder.Services.AddSingleton<IEmployeeMongoDbSettings>(
    provider => provider.GetRequiredService<IOptions<EmployeeMongoDbSettings>>().Value);

builder.Services.AddScoped<MemberService>();
builder.Services.AddScoped<OrganizationService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/organization/{organizationId}/members",
    (MemberService service, string organizationId) =>
        service.GetByOrganization(organizationId)
);

app.MapGet("/members",
    async (MemberService service) =>
        await service.GetAsync()
);

app.MapGet("/organizations",
    async (OrganizationService service) =>
        await service.GetAsync());

app.Run();