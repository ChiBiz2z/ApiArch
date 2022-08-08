using EmployeesAPI.Organizations.OrganizationRequests;
using Microsoft.AspNetCore.Authorization;

namespace EmployeesAPI.Organizations;

public static class OrganizationEndPoints
{
    public static WebApplication? MapOrganizationEndpoints(this WebApplication? app, string prefix)
    {
        if (app == null)
        {
            return app;
        }

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
            [Authorize]
            async (OrganizationService service, string id) =>
            {
                var request = new DeleteOrganizationRequest
                {
                    Key = id
                };

                return await service.RemoveAsync(request);
            }).WithTags("Organization");

        app.MapPost("/organizations/",
            [Authorize]
            async (OrganizationService service, CreateOrganizationRequest request) =>
                await service.CreateAsync(request)).WithTags("Organization");

        app.MapPut("/organizations/{id}",
            [Authorize]
            async (OrganizationService service, string id, UpdateOrganizationRequest request) =>
            {
                request.Key = id;
                return await service.UpdateAsync(request);
            }).WithTags("Organization");

        return app;
    }
}