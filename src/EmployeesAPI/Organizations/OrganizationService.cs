using EmployeesAPI.Account;
using EmployeesAPI.Account.AccountsRequests;
using EmployeesAPI.DAL.Repositories;
using EmployeesAPI.Domain;
using EmployeesAPI.Organizations.OrganizationRequests;

namespace EmployeesAPI.Organizations
{
    public class OrganizationService
    {
        private readonly OrganizationRepository _repository;
        private readonly UserService _userService;

        public OrganizationService(OrganizationRepository repository, UserService userService)
        {
            _repository = repository;
            _userService = userService;
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

        public async Task<IResult> GetOrganizations(GetManyOrganizationsRequest request)
        {
            var organizations =
                await _repository.GetAll(request.Search, (int)request.PageNumber, (int)request.PageSize);
            
            if (organizations == null || !organizations.Any())
                return Results.BadRequest();

            var response = new List<OrganizationViewModel>();
            foreach (var organization in organizations)
            {
                response.Add(new OrganizationViewModel
                {
                    Key = organization.Key,
                    Name = organization.Name
                });
            }

            return Results.Ok(response);
        }

        public async Task<IResult> CreateAsync(CreateOrganizationRequest request)
        {
            if (await _repository.ContainsName(request.Name))
                return Results.BadRequest();


            var organization = new Organization(request.Name);

            var user = new User(
                request.DefaultUserEmail,
                organization.Key
            );

            var res = await _repository.Create(organization);

            try
            {
                await _userService.Register(new RegisterUserRequest
                {
                    Email = user.Email,
                    Password = request.DefaultUserPassword,
                    OrganizationId = user.OrganizationId
                });
            }
            catch (Exception e)
            {
                try
                {
                    await _repository.Delete(organization.Key);
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Organization with key {organization.Key} has been created without User!");
                }
            }


            return res ? Results.Ok(organization) : Results.BadRequest();
        }

        public async Task<IResult> UpdateAsync(UpdateOrganizationRequest request)
        {
            var organisation = new Organization(request.Key, request.Name);

            var res = await _repository.Update(organisation);
            return res ? Results.Ok(organisation) : Results.BadRequest();
        }

        public async Task<IResult> RemoveAsync(DeleteOrganizationRequest request)
        {
            var res = await _repository.Delete(request.Key);
            return res ? Results.Ok() : Results.BadRequest();
        }
    }
}