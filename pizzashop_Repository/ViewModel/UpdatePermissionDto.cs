namespace pizzashop_Repository.ViewModel;

public class UpdatePermissionDto
{
    public int roleId {get; set;}
    public int  PermissionId{get; set;}
    public  string ? Type {get; set;}
    public bool IsChecked {get; set;}
}
