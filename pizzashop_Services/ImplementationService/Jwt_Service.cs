using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using pizzashop_Services.interfaceService;
namespace pizzashop_Services.ImplementationService;

public class Jwt_Service : IJwt_Service
{
   
    private readonly IConfiguration _configuration;
    public Jwt_Service(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GenerateJwtToken(string email, int roleId)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "admin");
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
          var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, roleId.ToString()),
                new Claim("roleId", roleId.ToString()),
            };
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public bool ValidateToken(string token, out JwtSecurityToken jwtSecurityToken)
    {
        try
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "");
            jwtSecurityToken = null;
            if (token == null)
                return false;
            var tokenHandler = new JwtSecurityTokenHandler();
            var validateParameter = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
            tokenHandler.ValidateToken(token, validateParameter, out SecurityToken validatedToken);
            jwtSecurityToken = (JwtSecurityToken)validatedToken;
            if (jwtSecurityToken != null)
            {
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
            jwtSecurityToken=null;
            return false;
        }
    }
}
