using Org.BouncyCastle.Asn1.Ocsp;
using pizzashop_Repository.Interface;
using pizzashop_Repository.Models;
using pizzashop_Repository.ViewModel;

namespace pizzashop_Services.ImplementationService;
public class Role_Services : IRole_Services
{
    private readonly IRolePermission_Repository rolePermission_Repository;
    public Role_Services(IRolePermission_Repository _rolePermission_Repository)
    {
        rolePermission_Repository = _rolePermission_Repository;
    }
    public List<RoleDto> GetAllRoles(){
        List<RoleDto> RoleList = rolePermission_Repository.getroles();
        return RoleList;
    }
    public List<RolePermissionDto> GetRolePermissions(int roleID){
        return rolePermission_Repository.GetPermissionByRole(roleID);
    }

    // public void UpdateRolePermissions(List<RolePermissionDto> updatedPermissions, string roleName)
    // {
    //     int roleID= rolePermission_Repository.GetRoleID(roleName);
    //     rolePermission_Repository.UpdateRolePermissions(updatedPermissions,roleID);
    // }
}