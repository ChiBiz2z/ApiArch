using EmployeesAPI.Configuration;
using EmployeesAPI.Members;
using EmployeesAPI.Organizations;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<MemberService>();
builder.Services.AddScoped<OrganizationService>();

builder.Services.ConfigureSwagger()
    .MongoDbConfiguration(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var basePrefix = "/api";
var organizationPrefix = "/organizations";

app.MapOrganizationEndpoints($"{basePrefix}{organizationPrefix}");
app.MapMemberEndpoints($"{basePrefix}");

app.Run();