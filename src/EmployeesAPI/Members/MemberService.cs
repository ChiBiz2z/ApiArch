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

        // public async Task<List<Member>> GetAsync() =>
        //     await _memberCollection.Find(_ => true).ToListAsync();

        // public async Task<List<Member>> GetByOrganization(string id)
        //     => await _memberCollection.Find(_ => _.OrganizationKey == id).ToListAsync();
        //
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

        public async Task<IResult> CreateAsync(CreateMemberRequest request)
        {
            //TODO
            var member = new Member(
                request.Name,
                request.Surname,
                request.Age,
                request.OrganizationKey
                );

            var res = await _repository.Create(member);

            return res ? Results.Ok(member) : Results.BadRequest();
        }


        // public async Task RemoveAsync(string id) =>
        //     await _memberCollection.DeleteOneAsync(m => m.Id == id);
        //
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