using System.ComponentModel.DataAnnotations;
using System.Data;

namespace pizzashop_Repository.ViewModel;
public class addNewModifier{
    [Required(ErrorMessage ="Field is empty")]
    public string? ModifiergroupName{get; set;}
    public string? Description{get; set;}
    public List<int>? SelectedModifiers{get; set;}
}   