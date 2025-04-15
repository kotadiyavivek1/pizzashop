using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using pizzashop_Services.interfaceService;

namespace pizzashop.Controllers;
public class TableController : Controller
{
    private readonly IJwt_Service jwtService;
    public TableController(IJwt_Service jwtService)
    {
        this.jwtService = jwtService;
    }
    public IActionResult Index()
    {
        ViewBag.ActiveNav = "Table";
        string token = HttpContext.Request.Cookies["JWTToken"] ?? "";
        if (!string.IsNullOrEmpty(token) && jwtService.ValidateToken(token, out var jwtToken))
        {
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role");
            ViewData["UserRole"] = roleClaim?.Value;
        }
        return View();
    }
}