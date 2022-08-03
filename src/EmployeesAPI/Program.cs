using EmployeesAPI.DAL.Interfaces;
using EmployeesAPI.DAL.Repositories;
using EmployeesAPI.Members;
using EmployeesAPI.Members.MemberRequests;
using EmployeesAPI.Models;
using EmployeesAPI.Organizations;
using EmployeesAPI.Organizations.OrganizationRequests;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<EmployeeMongoDbSettings>(
    builder.Configuration.GetSection(nameof(EmployeeMongoDbSettings)));

builder.Services.AddSingleton<IEmployeeMongoDbSettings>(
    provider => provider.GetRequiredService<IOptions<EmployeeMongoDbSettings>>().Value);

builder.Services.AddScoped<MemberService>();
builder.Services.AddScoped<OrganizationService>();
builder.Services.AddScoped<OrganizationRepository>();
builder.Services.AddScoped<MemberRepository>();

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
// app.MapGet("/api/organization/{organizationId}/members",
//     (MemberService service, string organizationId) =>
//         service.GetByOrganization(organizationId)
// ).WithTags("Members");
//
// app.MapGet("/members",
//     async (MemberService service) =>
//         await service.GetAsync()
// ).WithTags("Members");

// app.MapGet("/members/{id}",
//     async (MemberService service, string id) =>
//         await service.GetById(id)).WithTags("Members");
//
// app.MapDelete("/members/{id}",
//     async (MemberService service, string id) =>
//         await service.RemoveAsync(id)).WithTags("Members");
//
app.MapPost("/members/",
    async (MemberService service, CreateMemberRequest request) =>
        await service.CreateAsync(request)).WithTags("Members");

app.MapPut("/members/{id}",
    async (MemberService service, string id, UpdateMemberRequest request) =>
    {
        request.Key = id;
        await service.UpdateAsync(request);
    }).WithTags("Members");


//Organization
//app.MapGet("/organizations",
//    async (OrganizationService service) =>
//        await service.GetAsync()).WithTags("Organization");

app.MapGet("/organizations/{id}",
    async (OrganizationService service, string id) =>
    {
        var request = new GetOrganizationRequest
        {
            Key = id
        };
        return await service.GetById(request);
    }).WithTags("Organization");


app.MapDelete("/organizations/{id}",
    async (OrganizationService service, string id) =>
    {
        var request = new DeleteOrganizationRequest
        {
            Key = id
        };
        
        return await service.RemoveAsync(request);
    }).WithTags("Organization");

app.MapPost("/organizations/",
    async (OrganizationService service, CreateOrganizationRequest request) =>
        await service.CreateAsync(request)).WithTags("Organization");

app.MapPut("/organizations/{id}",
    async (OrganizationService service, string id, UpdateOrganizationRequest request) =>
    {
        request.Key = id;
        await service.UpdateAsync(request);
    }).WithTags("Organization");


app.Run();