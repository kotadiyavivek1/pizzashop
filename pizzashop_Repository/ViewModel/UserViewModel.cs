using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace pizzashop_Repository.ViewModel
{

   public class UserViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; }
   
    public string Email { get; set; }
    public string Phone { get; set; }
    public string RoleName { get; set; }
    public bool Status { get; set; }
    public string profileImage {get; set;}
}

}
