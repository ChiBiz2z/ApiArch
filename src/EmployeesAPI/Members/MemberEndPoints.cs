using EmployeesAPI.Members.MemberRequests;

namespace EmployeesAPI.Members;

public static class MemberEndPoints
{
    public static WebApplication? MapMemberEndpoints(this WebApplication? app, string prefix)
    {
        if (app == null)
            return app;

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


        return app;
    }
}