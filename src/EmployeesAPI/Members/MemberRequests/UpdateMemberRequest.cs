using System.ComponentModel.DataAnnotations;

namespace EmployeesAPI.Members.MemberRequests
{
    public class UpdateMemberRequest
    {
        public string Key { get; set; }
        [Required]
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        [Required]
        public string OrganizationKey { get; set; }
    }
}
