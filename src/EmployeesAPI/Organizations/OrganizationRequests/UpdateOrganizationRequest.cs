using System.ComponentModel.DataAnnotations;
using EmployeesAPI.Configuration;

namespace EmployeesAPI.Organizations.OrganizationRequests
{
    public class UpdateOrganizationRequest
    {
        [SwaggerIgnore]
        public string Key { get; set; }
        [Required]
        public string Name { get; set; }
        
        
    }
}
