using System.ComponentModel.DataAnnotations;
using EmployeesAPI.Configuration;

namespace EmployeesAPI.Members.MemberRequests
{
    public class UpdateMemberRequest
    {
        [SwaggerIgnore]
        public string Key { get; set; }
        [Required]
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        [Required]
        public string OrganizationKey { get; set; }
    }
}
