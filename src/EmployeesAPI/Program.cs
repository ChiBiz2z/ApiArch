using EmployeesAPI.Interfaces;
using EmployeesAPI.Models;
using EmployeesAPI.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

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

app.MapGet("/members/{id}",
    async (MemberService service, string id) =>
        await service.GetById(id));

app.MapDelete("/members/remove/{id}",
    async (MemberService service, string id) =>
        await service.RemoveAsync(id));

app.MapPost("/members/add/",
    async (MemberService service, Member member) =>
        await service.CreateAsync(member));

app.MapPut("/members/update/{id}",
    async (MemberService service, string id, Member member) =>
        await service.UpdateAsync(id, member));

//Organizaion

app.MapGet("/organizations",
    async (OrganizationService service) =>
        await service.GetAsync());

app.MapGet("/organizations/{id}",
    async (OrganizationService service, string id) =>
        await service.GetById(id));

app.MapDelete("/organizations/remove/{id}",
    async (OrganizationService service, string id) =>
        await service.RemoveAsync(id));

app.MapPost("/organizations/add/",
    async (OrganizationService service, Organization organization) =>
        await service.CreateAsync(organization));

app.MapPut("/organizations/update/{id}",
    async (OrganizationService service, string id, Organization organization) =>
        await service.UpdateAsync(id, organization));

app.Run();