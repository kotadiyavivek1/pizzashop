using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace pizzashop_Services.interfaceService;

public interface IJwt_Service
{
   public string GenerateJwtToken(string email,int roleId);
   public bool ValidateToken(string token, out JwtSecurityToken jwtSecurityToken);
}
