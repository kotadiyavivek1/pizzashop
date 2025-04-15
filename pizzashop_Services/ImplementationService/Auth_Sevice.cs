using Microsoft.AspNetCore.Http;
using pizzashop_Repository.Interface;
using pizzashop_Services.interfaceService;
using pizzashop_Repository.ViewModel;
using Microsoft.Extensions.Caching.Memory;


namespace pizzashop_Services.ImplementationService
{
    public class Auth_Sevice : IAuth_Service
    {
        private readonly IAuth_Repository _repository;
        private readonly IJwt_Service _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmail_Service _email_Service;


        public Auth_Sevice(IAuth_Repository repository,
                           IJwt_Service jwtService,
                           IHttpContextAccessor httpContextAccessor,
                           IEmail_Service email_Service,
                           IMemoryCache memoryCache)
        {
            _jwtService = jwtService;
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _email_Service = email_Service;
        }

        public async Task<bool> ChangePassword(ChangePasswordModel model, string email)
        {
            if (!_repository.VerifyUserEmail(email)) return false;

            if (model.NewPassword != model.ConfirmNewPassword) return false;

            if (string.IsNullOrEmpty(model.CurrentPassword) || string.IsNullOrEmpty(model.NewPassword))
                return false;

            return await _repository.changePassword(model.CurrentPassword, model.NewPassword, email);
        }

        public bool Login(LoginModel model, out string errormessage)
        {
            errormessage = string.Empty;
            if (!_repository.VerifyUserEmail(model.Email))
            {
                errormessage = "invalid Email";
                return false;
            }
            if (!_repository.VerifyUser(model.Email, model.Password))
            {
                errormessage = "invalid password";
                return false;
            }
            if (_repository.VerifyUser != null && _repository.VerifyUserEmail != null)
            {
                var token = _jwtService.GenerateJwtToken(model.Email, _repository.getRoleId(model.Email));
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(model.RememberMe ? 7 : 1), 
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                };
                
                var httpContext = _httpContextAccessor.HttpContext;
                httpContext?.Response.Cookies.Append("JWTToken", token, cookieOptions);
                httpContext?.Response.Cookies.Append("Email", model.Email, cookieOptions);
                return true;
            }
            return false;
        }

        public async Task<bool> SendEmail(ForgotPasswordModel model)
        {
            if (!_repository.VerifyUserEmail(model.Email)) return false;
            int Roleid = _repository.getRoleId(model.Email);
            string token = _jwtService.GenerateJwtToken(model.Email, Roleid);
            if (string.IsNullOrEmpty(token)) return false;
            _repository.saveToken(token, model.Email);
            string resetLink = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/Auth/ResetPassword?token={token}";
            string subject = "Reset Your Password";
            string htmlMessage = $"<p>Click <a href={resetLink}>here</a> to reset your password.</p>";
            await _email_Service.SendEmailAsync(model.Email, subject, htmlMessage);
            return true;
        }

        public async Task<bool> updatePassword(ResetPasswordModel model, string email)
        {
            try
            {
                if (model.Password != model.ConfirmPassword || string.IsNullOrEmpty(model.Password))
                {
                    Console.WriteLine("Passwords do not match or are empty.");
                    return false;
                }

                if (!_repository.VerifyUserEmail(email))
                {
                    Console.WriteLine("Invalid email.");
                    return false;
                }

                if (!_repository.VerifyResetToken(model.Token ?? "", email))
                {
                    Console.WriteLine("Invalid or expired reset token.");
                    return false;
                }

                await _repository.updatePassword(email, model.Password);
                Console.WriteLine("Password updated successfully");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating password: " + ex.Message);
                return false;
            }
        }
    }
}