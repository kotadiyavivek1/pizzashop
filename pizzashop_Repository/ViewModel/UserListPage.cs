using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace pizzashop_Repository.ViewModel
{

   public class UserListPage
{
    public List<UserViewModel> Users { get; set; }
    public int TotalUsers {get; set;}
    public int CurrentPage { get; set;}
    public int PageSize { get; set;}
    public int TotalPages { get; set;}
    public string SearchString{get; set;}

    public string SortOrder{get; set;}
}
}
