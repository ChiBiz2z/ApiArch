using EmployeesAPI.Models;
using EmployeesAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesAPI.Controllers
{
    [ApiController]
    [Route("api/members")]
    public class MemberController : ControllerBase
    {
        private readonly MemberService _memberService;

        public MemberController(MemberService memberService) =>
            _memberService = memberService;

        [HttpGet]
        public async Task<List<Member>> Get()
        {
            return await _memberService.GetAsync();
        }
    }
}
