namespace pizzashop_Repository.ViewModel;

public class RolePermissionDto
{
    public int RoleId { get; set; }
    public string? RoleName { get; set; }
    public int PermissionId { get; set; }
    public string? PermissionName { get; set; }
    public bool CanView { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
}
