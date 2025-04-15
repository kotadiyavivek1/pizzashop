using Microsoft.AspNetCore.Mvc;

namespace pizzashop.Controllers;
    public class AccessDeniedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
