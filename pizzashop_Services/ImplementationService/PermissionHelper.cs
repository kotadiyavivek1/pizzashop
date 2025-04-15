using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using pizzashop_Repository.Interface;
using pizzashop_Services.interfaceService;
namespace pizzashop_Services.ImplementationService;
public static class PermissionHelper
{
    public static bool HasPermission(HttpContext context, string requiredPermission, bool requireView = false, bool requireEdit = false, bool requireDelete = false)
    {
        var jwtService = context.RequestServices.GetService<IJwt_Service>();
        var roleService = context.RequestServices.GetService<IRole_Services>();

        var token = context.Request.Cookies["JWTToken"];
        if (string.IsNullOrEmpty(token) || jwtService == null || roleService == null)
            return false;

        if (!jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            return false;

        var roleIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role" || c.Type == ClaimTypes.Role);
        if (roleIdClaim == null || !int.TryParse(roleIdClaim.Value, out int roleId) || roleId == 0)
            return false;

        var permissions = roleService.GetRolePermissions(roleId);
        var permission = permissions.FirstOrDefault(p => p.PermissionName == requiredPermission);

        if (permission == null) return false;

        if (requireView && !permission.CanView) return false;
        if (requireEdit && !permission.CanEdit) return false;
        if (requireDelete && !permission.CanDelete) return false;

        return true;
    }
}