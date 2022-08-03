using EmployeesAPI.DAL.Repositories;
using EmployeesAPI.Domain;
using EmployeesAPI.Organizations.CreateOrganization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesAPI.Organizations
{
    public class OrganizationService
    {
        private readonly OrganizationRepository _repository;

        public OrganizationService(OrganizationRepository repository)
        {
            _repository = repository;
        }

        //public async Task<List<Organization>> GetAsync() =>
        //    await _organizationCollection.Find(_ => true).ToListAsync();

        //public async Task<IResult> GetById(string id)
        //{
        //    var organization = await _organizationCollection.Find(
        //        o => o.Id == id).FirstOrDefaultAsync();

        //    return organization != null ? Results.Ok(organization) : Results.NotFound();
        //}

        public async Task<IResult> CreateAsync(CreateOrganizationRequest request)
        {
            //TODO проверка на существующее имя
            var organization = new Organization(request.Name);

            var res = await _repository.Create(organization);
            return res ? Results.Ok(organization) : Results.BadRequest();
        }


        //public async Task RemoveAsync(string id) =>
        //    await _organizationCollection.DeleteOneAsync(o => o.Id == id);

        public async Task<IResult> UpdateAsync(UpdateOrganizationRequest request)
        {
            var organisation = new Organization(request.Key, request.Name);

            var res = await _repository.Update(organisation);
            return res ? Results.Ok(organisation) : Results.BadRequest();
        }
    }
}
