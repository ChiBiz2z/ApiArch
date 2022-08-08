using System.Text.RegularExpressions;

namespace EmployeesAPI.Domain;

public class User
{
    public string Key { get; }
    public string Email { get; }
    public DateTime CreatedAt { get; }
    public string OrganizationId { get; }


    private const string EmailRegex =
        @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

    public User(string email, string organizationId)
    {
        if (!Regex.IsMatch(email.ToLower(), EmailRegex))
            throw new ArgumentNullException("Email не валиден");

        if (!Guid.TryParse(organizationId, out _))
            throw new ArgumentNullException("Id организации сотрудника не валиден");


        Email = email;
        OrganizationId = organizationId;
        CreatedAt = DateTime.Now;
        Key = Guid.NewGuid().ToString();
    }
}