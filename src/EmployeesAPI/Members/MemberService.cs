using EmployeesAPI.DAL.Repositories;
using EmployeesAPI.Domain;
using EmployeesAPI.Members.MemberRequests;

namespace EmployeesAPI.Members
{
    public class MemberService
    {
        private readonly MemberRepository _repository;

        public MemberService(MemberRepository repository)
        {
            _repository = repository;
        }

        public async Task<IResult> GetById(GetMemberRequest request)
        {
            var model = await _repository.GetById(request.Key);
            if (model == null)
                return Results.NotFound();

            var response = new MemberViewModel
            {
                Key = model.Key,
                Name = model.Name,
                Surname = model.Surname,
                Age = model.Age,
                OrganizationKey = model.OrganizationKey
            };

            return Results.Ok(response);
        }

        public async Task<IResult> GetMembers(GetManyMembersRequest request)
        {
            var members = await _repository.GetAll(request.FullNameSearch, (int)request.AgeFilterFrom, (int)request.AgeFilterTo, (int)request.PageNumber, (int)request.PageSize);
            if(members == null || !members.Any())
                return Results.BadRequest();

            var response = new List<MemberViewModel>();
            foreach (var member in members)
            {
                response.Add(new MemberViewModel
                {
                    Age = member.Age,
                    Name = member.Name,
                    OrganizationKey = member.OrganizationKey,
                    Key = member.Key,
                    Surname = member.Surname
                });
            }
            
            return Results.Ok(response);
        }

        public async Task<IResult> CreateAsync(CreateMemberRequest request)
        {
            var member = new Member(
                request.Name,
                request.Surname,
                request.Age,
                request.OrganizationKey
            );

            var res = await _repository.Create(member);

            return res ? Results.Ok(member) : Results.BadRequest();
        }


        public async Task<IResult> RemoveAsync(DeleteMemberRequest request)
        {
            var res = await _repository.Delete(request.Key);
            return res ? Results.Ok() : Results.BadRequest();
        }

        public async Task<IResult> UpdateAsync(UpdateMemberRequest request)
        {
            var member = new Member(
                request.Key,
                request.Name,
                request.Surname,
                request.Age,
                request.OrganizationKey
            );

            var res = await _repository.Update(member);

            return res ? Results.Ok(member) : Results.BadRequest();
        }
    }
}