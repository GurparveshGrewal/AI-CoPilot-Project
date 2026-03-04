using System.Security.Cryptography;
using System.Text;
using Api.Models;

public class AuthService : IAuthService
{
    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        return Convert.ToBase64String(randomBytes);
    }

    public string HashToken(string token)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));

        return Convert.ToBase64String(bytes);
    }

    public string GenerateAccessToken(User user)
    {
        // We will implement JWT logic next
        throw new NotImplementedException();
    }
}