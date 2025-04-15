using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using pizzashop_Repository.Models;
using pizzashop_Repository.ViewModel;

namespace pizzashop_Repository.Implementation;
public interface IUser_Repository
{
    public IQueryable<User> GetUsers(string searchString, string sortOrder, int pageNumber, int pageSize, out int totalRecords);
    public User GetUserById(int userId);
    public void AddUser(User user);
    public void SaveChanges();

    public bool IsDeleteDb(int id);
    public ProfileModel GetUserByEmail(string email);
    public bool UpdateUser(ProfileModel model, string profilePicturePath);
    public User GetUserDtoByEmail(string email);
    public List<countryDto> GetAllCountries();
    public List<stateDto> GetAllStates(int countryid);
    public List<cityDto> GetAllCities(int stateid);
    public AddNewUserModel GetUserDetail(int userid);
    public bool updateUser(AddNewUserModel model,string profilePicturePath);
    List<UserViewModel> GetUsers(string? searchString, int pageNumber, int pageSize, string? sortOrder);
    int GetTotalUserCount(string? searchString);
}
