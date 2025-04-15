using System.ComponentModel.DataAnnotations;

namespace pizzashop_Repository.ViewModel;

public class LoginModel
{
    [Required]
    public string Email { get; set; } = null!;
    
    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public bool RememberMe { get; set;}
}
