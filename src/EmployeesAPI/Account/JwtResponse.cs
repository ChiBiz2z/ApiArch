using System.Text.Json.Serialization;

namespace EmployeesAPI.Account;

public record JwtResponse
{
    [JsonPropertyName("token")]
    public string Token { get; set; }
};