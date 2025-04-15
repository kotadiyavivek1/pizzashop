using pizzashop_Repository.Models;
using pizzashop_Services.ImplementationService;
using pizzashop_Repository.ViewModel;
namespace pizzashop_Services.interfaceService;

public interface IAuth_Service
{
    public bool Login(LoginModel model,out string errormessage);
    Task<bool> SendEmail(ForgotPasswordModel model);
    Task<bool> updatePassword(ResetPasswordModel model,string email);
    Task<bool> ChangePassword(ChangePasswordModel model,string email);
}
