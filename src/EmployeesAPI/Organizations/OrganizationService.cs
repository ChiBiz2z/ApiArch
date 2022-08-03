using EmployeesAPI.DAL.Repositories;
using EmployeesAPI.Domain;
using EmployeesAPI.Organizations.OrganizationRequests;

namespace EmployeesAPI.Organizations
{
    public class OrganizationService
    {
        private readonly OrganizationRepository _repository;

        public OrganizationService(OrganizationRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<IResult> GetById(GetOrganizationRequest request)
        {
            var model = await _repository.GetById(request.Key);
            if (model == null)
                return Results.NotFound();

            var response = new OrganizationViewModel
            {
                Key = model.Key,
                Name = model.Name
            };

            return Results.Ok(response);
        }

        public async Task<IResult> CreateAsync(CreateOrganizationRequest request)
        {
            //TODO проверка на существующее имя
            var organization = new Organization(request.Name);

            var res = await _repository.Create(organization);
            return res ? Results.Ok(organization) : Results.BadRequest();
        }

        public async Task<IResult> UpdateAsync(UpdateOrganizationRequest request)
        {
            var organisation = new Organization(request.Key, request.Name);

            var res = await _repository.Update(organisation);
            return res ? Results.Ok(organisation) : Results.BadRequest();
        }
    }
}
