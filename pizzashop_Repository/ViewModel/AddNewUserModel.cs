using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace pizzashop_Repository.ViewModel;
public class AddNewUserModel
{
    [Required(ErrorMessage = "First Name is required.")]
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    [Required(ErrorMessage = "User Name is required.")]
    public string? UserName { get; set; }
    [Required(ErrorMessage = "role is required.")]
    public  int roleID { get; set; }
    [Required(ErrorMessage = "Email is required.")]
    public  string? Email { get; set; }
    [Required(ErrorMessage = "Password is required.")]
    public  string? Password { get; set; }
    public bool? Status{get; set;}
    public int? CountryID { get; set; }
    public int? StateID { get; set; }
    public int? CityID { get; set; }
    public string? ZipCode { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public IFormFile? ProfilePicture { get; set; }
    public string? ProfilePicturePath { get; set; } 
    public List<countryDto>? countries { get; set; }
    public List<cityDto>? cities{ get; set; }
    public List<stateDto>? states { get; set; }
}
