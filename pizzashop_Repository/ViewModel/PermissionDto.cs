using System.ComponentModel.DataAnnotations;

namespace pizzashop_Repository.ViewModel;

public class PermissionDto
{
    public int Id { get; set; }
    [Required]
    public string PermissionName { get; set; }
}
