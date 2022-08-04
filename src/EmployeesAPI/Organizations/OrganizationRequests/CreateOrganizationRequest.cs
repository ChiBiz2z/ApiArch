using System.ComponentModel.DataAnnotations;

namespace EmployeesAPI.Organizations.OrganizationRequests
{
    public class CreateOrganizationRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
