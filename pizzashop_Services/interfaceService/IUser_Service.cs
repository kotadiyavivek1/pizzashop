using System.Runtime.CompilerServices;
using pizzashop_Repository.ViewModel;
using pizzashop_Services.ImplementationService;

namespace pizzashop_Services.interfaceService;

public interface IUser_Service
{
    ProfileModel profileView(string Email);
    IEnumerable<UserViewModel> GetUsers(string searchString, string sortOrder,int pageNumber, int pageSize, out int totalRecords); 
    Task<bool> AddUser(AddNewUserModel model, string currentUserEmail);
    bool UpdateUser(ProfileModel model);
    bool IsDelete(int id);
    ProfileModel GetProfileByEmail(string email);
    List<countryDto> GetCountries();
    List<stateDto> GetStates(int countryId);
    List<cityDto> GetCities(int stateID);
    AddNewUserModel getUserById(int id);
    bool UpdateUserDb(AddNewUserModel model);
    List<UserViewModel> GetUsers(string? searchString, int pageNumber, int pageSize, string? sortOrder);
    int GetTotalUserCount(string? searchString);
}
