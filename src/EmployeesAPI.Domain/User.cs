namespace EmployeesAPI.Domain;

public class User
{
    public string Key { get; }
    public string Email { get; }
    public string PasswordHash { get; }
    public string PasswordKey { get; }
    public DateTime CreatedAt { get; }
    public string OrganizationId { get; }

    public User(string email, string passwordHash, string organizationId, string passwordKey)
    {
        Email = email;
        PasswordHash = passwordHash;
        OrganizationId = organizationId ?? "OrganizationId";
        PasswordKey = passwordKey;
        CreatedAt = DateTime.Now;
        Key = Guid.NewGuid().ToString();
    }
}