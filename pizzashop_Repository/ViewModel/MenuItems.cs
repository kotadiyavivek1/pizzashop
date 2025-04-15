using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace pizzashop_Repository.ViewModel;

public class MenuItems
{
    public int itemId { get; set; }

    [Required(ErrorMessage = "Unit is required.")]
    public int UnitId { get; set; }
    public string? UnitName { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Category Name is required.")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Type is required.")]
    public bool Type { get; set; }

    [Required(ErrorMessage = "Rate is Required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Rate must be greater than 0.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    public int Quantity { get; set; }

    public bool IsAvailable { get; set; }
    public bool IsDefaultTax { get; set; }

    [Range(0, 100, ErrorMessage = "Tax Percentage must be between 0 and 100.")]
    public decimal TaxPercentage { get; set; }

    public string? ShortCode { get; set; }
    public string? Description { get; set; }

    public IFormFile? Image { get; set; }
    public string? ImagePath { get; set; }

    public bool Isfavourite { get; set; }

    // Modifier Group Section
    public List<ModifiergroupDto>? Modifiergroup { get; set; }
}
