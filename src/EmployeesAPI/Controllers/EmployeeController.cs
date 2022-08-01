using EmployeesAPI.Models;
using EmployeesAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesAPI.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService employeeService;

        public EmployeeController(EmployeeService employeeService) =>
            this.employeeService = employeeService;

        [HttpGet]
        public async Task<List<Employee>> Get() =>
            await employeeService.GetAsync();
    }
}
