using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace pizzashop_Repository.ViewModel;

public class TableDto
{
    public int Id { get; set; }
    public int SectionId { get; set; }  
    [Required(ErrorMessage = "Name must be required.")]
    public string? TableName{ get; set; }
    public bool IaAvailable{get; set;}
    [Required(ErrorMessage = "Capacity must be required.")]
    [Range(1,100,ErrorMessage ="Capacity must be greater than zero and less than 100")]
    public int Capacity{get; set;}
}
