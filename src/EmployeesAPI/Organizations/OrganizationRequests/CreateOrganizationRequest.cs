using System.ComponentModel.DataAnnotations;

namespace EmployeesAPI.Organizations.OrganizationRequests
{
    public class CreateOrganizationRequest
    {
        [Required]
        public string Name { get; set; }

        public string DefaultUserEmail { get; set; }

        public string DefaultUserPassword { get; set; }
    }
}
