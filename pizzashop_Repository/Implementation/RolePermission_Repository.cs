using System.Linq;
using System.Security.Cryptography.X509Certificates;
using pizzashop_Repository.Interface;
using pizzashop_Repository.Models;
using pizzashop_Repository.ViewModel;

namespace pizzashop_Repository.Implementation;

public class RolePermission_Repository : IRolePermission_Repository
{
    private readonly PizzashopContext _context;
    public RolePermission_Repository(PizzashopContext context)
    {
        _context = context;
        // }
        // public  getRoles();
    }

    public List<RolePermissionDto> GetPermissionByRole(int roleID)
    {
        return _context.Rolepermissions
            .Where(rp => rp.Roleid == roleID).Select(rp => new RolePermissionDto
            {
                RoleId = rp.Roleid,
                RoleName = rp.Role.Name,
                PermissionId = rp.Permissionid,
                PermissionName = rp.Permission.Name,
                CanView = rp.Canview ?? false,
                CanEdit = rp.Canedit ?? false,
                CanDelete = rp.Candelete ?? false,
            }).ToList();
    }




    public int GetRoleID(string roleName)
    {
        var role = _context.Roles.FirstOrDefault(r=>r.Name == roleName);
        if (role == null)
        {
            return 0; // Role not found
        }   
        return role.Id;
    }

    public List<RoleDto> getroles()
    {
        var RolesData = _context.Roles.Select(r => new RoleDto
        {
            Id = r.Id,
            roleName = r.Name
        }).ToList();

        return RolesData;
    }


    // public void UpdateRolePermissions(List<RolePermissionDto> updatedPermissions,int roleID)
    // {
    //     foreach (var permission in updatedPermissions)
    //     {
    //         var dbPermission = _context.Rolepermissions
    //             .FirstOrDefault(rp => rp.Permission.Name == permission.PermissionName && rp.Role.Id==roleID);

    //         if (dbPermission != null)
    //         {
    //             dbPermission.Canview = permission.CanView;
    //             dbPermission.Canedit = permission.CanEdit;
    //             dbPermission.Candelete = permission.CanDelete;
    //         }
    //     }
    //     _context.SaveChanges();
    // }
}
