using System.ComponentModel.DataAnnotations;

namespace EmployeesAPI.Organizations.OrganizationRequests
{
    public class UpdateOrganizationRequest
    {
        public string Key { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
