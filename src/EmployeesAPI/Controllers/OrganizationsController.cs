using EmployeesAPI.Models;
using EmployeesAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationsController : ControllerBase
    {
        private readonly OrganizationService _organizationService;
        private readonly MemberService _memberService;

        public OrganizationsController(OrganizationService organizationService) =>
            _organizationService = organizationService;

        [HttpGet]
        public async Task<List<Organization>> Get()
        {
            return await _organizationService.GetAsync();
        }


        [HttpGet("{organizationId}")]
        public async Task<List<Member>> GetMembers(string id) =>
            await _memberService.GetByOrganization(id);
    }
}
