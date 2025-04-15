using pizzashop_Repository.Models;
using pizzashop_Repository.ViewModel;
namespace pizzashop_Repository.Interface;
public interface IRolePermission_Repository{
    public List<RoleDto>  getroles();
    public List<RolePermissionDto> GetPermissionByRole(int roleID);
    // public void UpdateRolePermissions(List<RolePermissionDto> updatedPermissions,int roleID);
    public int GetRoleID(string roleName);
//     public List<string> GetPermissionByRoleID(int roleId);
    
}