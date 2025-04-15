using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pizzashop.Models;
using pizzashop_Services.interfaceService;
using pizzashop_Repository.ViewModel;
using System.IdentityModel.Tokens.Jwt;

namespace pizzashop.Controllers;

public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuth_Service _authService;

    public AuthController(ILogger<AuthController> logger, IAuth_Service authService)
    {
        _logger = logger;
        _authService = authService;
    }


    [HttpGet]
    public IActionResult CheckSession()
    {
        var token = Request.Cookies["JWTToken"]; 
        
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized(new { message = "Session expired" });
        }

        var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        try
        {
            var jwtToken = tokenHandler.ReadJwtToken(token);

            if (jwtToken.ValidTo < DateTime.UtcNow)
            {
                Response.Cookies.Delete("JWTToken");
                return Unauthorized(new { message = "Token expired" });
            }
        }
        catch (Exception)
        {
            return Unauthorized(new { message = "Invalid token" });
        }

        return Ok(new { message = "Session active" });
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            if (_authService.Login(model, out string errormessage))
            {
                TempData["Success"] = "Login successful! welcome to the dashboard";
                return RedirectToAction("Dashboard", "User");
            }
            TempData["Error"] = errormessage;
            return View(model);
        }
        return View(model);
    }

    public IActionResult ForgotPassword()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> ResetEmail(ForgotPasswordModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("ForgotPassword", model);
        }

        if (await _authService.SendEmail(model))
        {
            TempData["Success"] = "Email sent successfully.";
            return RedirectToAction("Login");
        }
        else
        {
            ModelState.AddModelError("Email", "Invalid User or Email not found.");
            return View("ForgotPassword", model);
        }
    }



    [HttpGet]
    public IActionResult ResetPassword(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value ?? "";
        ResetPasswordModel model = new()
        {
            Token = token,
            Email = email
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> UpdatePassword(ResetPasswordModel model)
    {

        if (ModelState.IsValid)
        {
            var isUpdated = await _authService.updatePassword(model, model.Email ?? "");
            try
            {
                if (isUpdated)
                {
                    TempData["Success"] = "Your Password has been changed successfully";
                    return RedirectToAction("Login");
                }
                TempData["Error"] = "Error resetting password. Please try again";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Invalid or expired token.";
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            return RedirectToAction("ResetPassword");
        }
        return View(model);
    }

    public IActionResult ChangePassword()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
    {
        if (ModelState.IsValid)
        {
            var email = Request.Cookies["Email"];
            if (email != null)
            {
                if (await _authService.ChangePassword(model, email))
                {
                    Console.WriteLine("Password Changed");
                    return RedirectToAction("ChangePassword", "Auth");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "your currentPassword is Wrong");
                    return RedirectToAction("ChangePassword", "Auth");
                }
            }
        }
        return View(model);
    }

    public IActionResult Logout()
    {
        var jwttoken = Request.Cookies["JWTToken"];
        if (jwttoken != null)
        {
            Response.Cookies.Delete("JWTToken");
            Response.Cookies.Delete("Email");
            TempData["Success"] = "Logout successfull";
            return RedirectToAction("Login", "Auth");
        }
        else
        {
            return View("Login");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

