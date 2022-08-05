using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace EmployeesAPI.Infrastructure;

public class SecurityService
{
    public static (string passwordHash, string passwordSalt) HashString(string value, string key = "")
    {
        HMACSHA512 hmac = new HMACSHA512();

        if (string.IsNullOrEmpty(key))
            key = Convert.ToBase64String(hmac.Key);

        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
        return (Convert.ToBase64String(hashBytes), key);
    }

    public static string GenerateJwtToken(string key, string issuer, string audience, string userId,
        string organizationId)
    {
        var handler = new JwtSecurityTokenHandler();
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);


        var claims = new Claim[]
        {
            new Claim(ClaimTypes.Name, userId),
            new Claim("organizationId", organizationId)
        };

        var jwtDescriptor = new SecurityTokenDescriptor
        {
            Audience = audience,
            Issuer = issuer,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha512Signature)
        };
        var token = handler.CreateToken(jwtDescriptor);
        return handler.WriteToken(token);
    }
}