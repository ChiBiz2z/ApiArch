using EmployeesAPI.Models;
using EmployeesAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesAPI.Controllers
{
    [ApiController]
    [Route("api/organizations")]
    public class OrganizationsController : ControllerBase
    {
        private readonly OrganizationService _organizationService;
        private readonly MemberService _memberService;

        public OrganizationsController(OrganizationService organizationService, MemberService memberService)
        {
            _organizationService = organizationService;
            _memberService = memberService;
        }


        [HttpGet]
        public async Task<List<Organization>> Get()
        {
            return await _organizationService.GetAsync();
        }


        [HttpGet("{organizationId}/members")]
        public async Task<List<Member>> GetMembers(string organizationId) =>
            await _memberService.GetByOrganization(organizationId);
    }
}
