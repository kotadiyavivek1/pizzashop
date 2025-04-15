
using System.ComponentModel.DataAnnotations;

namespace pizzashop_Repository.ViewModel;

public class ModifiersDto
{
    public int id {get; set;}
    public int modifierGroupID{get; set;}
    public string? name{get; set;}
    public string? unit{get; set;}
    public decimal rate{get; set;}
    public int? quantity{get; set;}
    public string? description{get; set;}
}