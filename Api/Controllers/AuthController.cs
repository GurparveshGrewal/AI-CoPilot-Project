using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            // --- PART 1: PREPARING THE "STAMP" (The Secret Key) ---

            // We need a "secret password" that only the server knows.
            // This key is used to sign the token. If a hacker tries to change the token,
            // the server will know because the signature won't match this secret key.
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("ThisIsMySuperSecretKey123456789012")
            );

            // This tells the computer: "Use the Secret Key above and the HMAC SHA256
            // math formula to create a digital lock on our token."
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // --- PART 2: CREATING THE "VISITOR PASS" (The JWT) ---

            // This creates the actual Token object.
            // 'expires' is crucial: we make it short (30 mins) so that if someone
            // steals this token, it becomes useless very quickly.
            var token = new JwtSecurityToken(
                claims: null, // Usually, you'd put the User's Email or Role here.
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            // This takes that 'token' object and turns it into a long string of
            // gibberish (Header.Payload.Signature) that the browser can understand.
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            // --- PART 3: THE "VIP KEY" (The Refresh Token) ---

            // Since the JWT only lasts 30 minutes, we don't want to kick the user out
            // and make them log in again. We generate a 'Refresh Token'—a long,
            // random string—that the user can swap for a fresh JWT later.
            var refreshToken = _authService.GenerateRefreshToken();

            // --- PART 4: RECORD KEEPING (The Database) ---

            // We need to remember that we gave this specific user a Refresh Token.
            // If we don't save it in the DB, we can't verify it later when they
            // come back to ask for a new JWT.
            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(), // A unique ID for this specific database row.

                // This links the token to a specific person. (Currently hardcoded for your test).
                UserId = Guid.Parse("1d511e1f-c2bd-40ab-80cc-7dec235682bf"),

                // SECURITY TIP: We never save the real Refresh Token in the DB.
                // We "Hash" it (scramble it). If a hacker steals your database,
                // they still can't use the tokens because they only have the scrambled version!
                TokenHash = _authService.HashToken(refreshToken),

                CreatedAt = DateTime.UtcNow, // Note when it was created.
                ExpiresAt = DateTime.UtcNow.AddDays(7), // This 'VIP Key' lasts for 7 days.
            };

            // Tell the Database context to prepare to save this new token.
            _context.RefreshToken.Add(refreshTokenEntity);

            // Actually save the data to your SQL/Postgres database.
            await _context.SaveChangesAsync();

            // --- PART 5: THE RESPONSE ---

            // Send the JWT back to the user's computer/phone.
            // NOTE: You should also send the 'refreshToken' string here so the
            // frontend can save it in its storage!
            return Ok(new { token = jwt, refreshToken = refreshToken });
        }
    }
}
