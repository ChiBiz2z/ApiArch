using System.ComponentModel.DataAnnotations;

namespace EmployeesAPI.Organizations.CreateOrganization
{
    public class CreateOrganizationRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
