using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

public class ModifierGroupDto
{
    public int id {get; set;}
    [Required]
    public string? ModifiergroupName{get; set;}

}