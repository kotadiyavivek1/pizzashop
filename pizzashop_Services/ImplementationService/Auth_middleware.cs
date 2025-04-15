using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using pizzashop_Repository.Interface;
using pizzashop_Repository.ViewModel;
using pizzashop_Services.interfaceService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class Auth_middleware{
public class CustomAuthorize : Attribute, IAuthorizationFilter
{
    private readonly string _requiredPermission;
    private readonly bool _requireView;
    private readonly bool _requireEdit;
    private readonly bool _requireDelete;
    public CustomAuthorize(string requiredPermission, bool requireView = false, bool requireEdit = false, bool requireDelete = false)
    {
        _requiredPermission = requiredPermission;
        _requireView = requireView;
        _requireEdit = requireEdit;
        _requireDelete = requireDelete;
    }
    
    public void OnAuthorization(AuthorizationFilterContext context)
        {
        var serviceProvider = context.HttpContext.RequestServices;

        var jwtService = serviceProvider.GetService<IJwt_Service>();
        if (jwtService == null)
        {
            context.Result = new RedirectToActionResult("Login", "Auth", null);
            return;
        }

        var token = context.HttpContext.Request.Cookies["JWTToken"];
        if (string.IsNullOrEmpty(token) || !jwtService.ValidateToken(token, out JwtSecurityToken jwtSecurityToken))
        {
            context.Result = new RedirectToActionResult("Login", "Auth", null);
            return;
        }

        var roleIdClaim = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "role" || c.Type == ClaimTypes.Role);

        if (roleIdClaim == null || !int.TryParse(roleIdClaim.Value, out int roleId) || roleId == 0)
        {
            context.Result = new RedirectToActionResult("Login", "Auth", null);
            return;
        }

        var roleService = serviceProvider.GetService<IRole_Services>();
        if (roleService == null)
        {
            context.Result = new RedirectToActionResult("Login", "Auth", null);
            return;
        }

        List<RolePermissionDto> permissions = roleService.GetRolePermissions(roleId);
        var permission = permissions.FirstOrDefault(p => p.PermissionName == _requiredPermission);

        if (permission == null || !permission.CanView)
        {
            context.Result=new RedirectToActionResult("Index","AccessDenied",null);
        }

        if (_requireView && !permission.CanView)
        {
            context.Result=new RedirectToActionResult("Index","AccessDenied",null);
        }

        if (_requireEdit && !permission.CanEdit)
        {
            context.Result=new RedirectToActionResult("Index","AccessDenied",null);
        }

        if (_requireDelete && !permission.CanDelete)
        {
            context.Result=new RedirectToActionResult("Index","AccessDenied",null);
        }
    }
}
}