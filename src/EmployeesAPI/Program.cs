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

//Member
app.MapGet("/api/organization/{organizationId}/members",
    (MemberService service, string organizationId) =>
        service.GetByOrganization(organizationId)
).WithTags("Members");

app.MapGet("/members",
    async (MemberService service) =>
        await service.GetAsync()
).WithTags("Members");

app.MapGet("/members/{id}",
    async (MemberService service, string id) =>
        await service.GetById(id)).WithTags("Members");

app.MapDelete("/members/remove/{id}",
    async (MemberService service, string id) =>
        await service.RemoveAsync(id)).WithTags("Members");

app.MapPost("/members/add/",
    async (MemberService service, Member member) =>
        await service.CreateAsync(member)).WithTags("Members");

app.MapPut("/members/update/{id}",
    async (MemberService service, string id, Member member) =>
        await service.UpdateAsync(id, member)).WithTags("Members");

//Organization
app.MapGet("/organizations",
    async (OrganizationService service) =>
        await service.GetAsync()).WithTags("Organization");

app.MapGet("/organizations/{id}",
    async (OrganizationService service, string id) =>
        await service.GetById(id)).WithTags("Organization");

app.MapDelete("/organizations/remove/{id}",
    async (OrganizationService service, string id) =>
        await service.RemoveAsync(id)).WithTags("Organization");

app.MapPost("/organizations/add/",
    async (OrganizationService service, Organization organization) =>
        await service.CreateAsync(organization)).WithTags("Organization");

app.MapPut("/organizations/update/{id}",
    async (OrganizationService service, string id, Organization organization) =>
        await service.UpdateAsync(id, organization)).WithTags("Organization");

app.Run();