using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SRMS.Utils
{
    public class TokenProvider
    {

        public string? GetAccessToken(Claim[] claims, string secretKey)
        {
            if (claims == null)
                return null;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
            );

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.WriteToken(token);
                return jwtToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public IEnumerable<Claim?> GetClaims(string? token)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var Token = tokenHandler.ReadJwtToken(token);

            return Token.Claims;
        }

    }
}
