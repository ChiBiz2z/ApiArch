using EmployeesAPI.Configuration;
using EmployeesAPI.DAL.Interfaces;
using EmployeesAPI.DAL.Repositories;
using EmployeesAPI.Members;
using EmployeesAPI.Members.MemberRequests;
using EmployeesAPI.Models;
using EmployeesAPI.Organizations;
using EmployeesAPI.Organizations.OrganizationRequests;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<MemberService>();
builder.Services.AddScoped<OrganizationService>();

builder.Services.ConfigureSwagger().MongoDbConfiguration(builder.Configuration);

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

//
app.MapDelete("/members/{id}",
    async (MemberService service, string id) =>
    {
        var request = new DeleteMemberRequest
        {
            Key = id
        };

        return await service.RemoveAsync(request);
    }).WithTags("Members");


app.MapGet("/members/{id}",
    async (MemberService service, string id) =>
    {
        var request = new GetMemberRequest
        {
            Key = id
        };

        return await service.GetById(request);
    }).WithTags("Members");

app.MapPost("/members/",
    async (MemberService service, CreateMemberRequest request) =>
        await service.CreateAsync(request)).WithTags("Members");

app.MapPut("/members/{id}",
    async (MemberService service, string id, UpdateMemberRequest request) =>
    {
        request.Key = id;
        await service.UpdateAsync(request);
    }).WithTags("Members");


var basePrefix = "/api";
var organizationPrefix = "/organizations";

app.MapOrganizationEndpoints($"{basePrefix}{organizationPrefix}");

app.Run();