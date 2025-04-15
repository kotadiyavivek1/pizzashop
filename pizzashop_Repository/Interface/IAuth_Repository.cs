using pizzashop_Repository.ViewModel;
namespace pizzashop_Repository.Interface;

public interface IAuth_Repository
{
   public bool VerifyUser(string email, string password);
   public string getRoleName(int roleID);
   public int getRoleId(string email);
   public bool VerifyUserEmail(string email);
   
   public Task updatePassword(string email,string password);
   public Task<bool> changePassword(string currentPassword,string NewPassword,string email);
   ProfileModel ViewDataUser(string Email);
   void saveToken(string Email,string token);
   bool VerifyResetToken(string Token,string email);
   
}
