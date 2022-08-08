namespace EmployeesAPI.Domain;

public class VerificationCode
{
    public string Email { get;}
    public string Code { get; }
    public DateTime GeneratedAt { get;}

    public VerificationCode(string email)
    {
        Email = email;
        Code = Guid.NewGuid().ToString();
        GeneratedAt = DateTime.UtcNow;
    }
}