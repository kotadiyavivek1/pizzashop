using System.ComponentModel.DataAnnotations;

namespace pizzashop_Repository.ViewModel;

public class SectionDto
{
    public int Id{get; set;}
    [Required(ErrorMessage = "SectionName must be required")]
    public string? SectionName{get; set;}
    public string? Description{get; set;}
}
