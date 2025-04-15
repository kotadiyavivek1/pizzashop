using Microsoft.AspNetCore.Http;

public class ProfileModel
{
    public int Id {get; set;}
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    public string? UserName { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? Zipcode { get; set; }
    public string? profileImage { get; set; } 
    public IFormFile? ProfileImageFile { get; set; } 
    public int? StateID{get; set;}
    public int? CountryID{get; set;}
    public int? CityID{get; set;}
}
