using System.ComponentModel.DataAnnotations;
namespace pizzashop_Repository.ViewModel;
public class ForgotPasswordModel{
    [Required]
    public string Email {get;set;} = null!;
}
