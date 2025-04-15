using System.ComponentModel.DataAnnotations;

namespace pizzashop_Repository.ViewModel;

public class AddCategory
{
    [Required(ErrorMessage = "Category Name is required.")]
    [MinLength(1, ErrorMessage = "Category Name cannot be empty.")]
    public string? CategoryName { get; set; }
    public string? Description { get; set; }
}
