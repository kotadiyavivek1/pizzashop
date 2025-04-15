using pizzashop_Repository.ViewModel;
namespace pizzashop_Repository.Interface;
public interface IRole_Services {
    public List<RoleDto> GetAllRoles();
    public List<RolePermissionDto> GetRolePermissions(int roleID);


}