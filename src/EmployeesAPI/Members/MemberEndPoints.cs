using EmployeesAPI.Members.MemberRequests;
using Microsoft.AspNetCore.Authorization;

namespace EmployeesAPI.Members;

public static class MemberEndPoints
{
    public static void MapMemberEndpoints(this WebApplication? app, string prefix)
    {
        if (app == null) return;

        app.MapDelete("/members/{id}",
            [Authorize] async (MemberService service, string id) =>
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

        app.MapGet("/members/search",
            async (MemberService service, string? name, int? ageFrom, int? ageTo, int? pageNumber, int? pageSize) =>
            {
                var request = new GetManyMembersRequest
                {
                    FullNameSearch = name,
                    AgeFilterFrom = ageFrom,
                    AgeFilterTo = ageTo,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return await service.GetMembers(request);
            }).WithTags("Members");

        app.MapPost("/members/",
            [Authorize] async (MemberService service, CreateMemberRequest request) =>
                await service.CreateAsync(request)).WithTags("Members");

        app.MapPut("/members/{id}",
            [Authorize(Policy = "Default")] async (MemberService service, string id, UpdateMemberRequest request) =>
            {
                request.Key = id;
                await service.UpdateAsync(request);
            }).WithTags("Members");
    }
}