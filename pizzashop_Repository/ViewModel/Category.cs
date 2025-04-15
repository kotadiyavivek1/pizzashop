using System.ComponentModel.DataAnnotations;

namespace pizzashop_Repository.ViewModel;

public class Category
{
   public int Categoryid {get; set;}
   [Required]
   public  string?  CategoryName {get; set;}

   public string? Description{get; set;}
}
