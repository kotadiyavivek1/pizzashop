using Microsoft.AspNetCore.Mvc;
using pizzashop_Services.interfaceService;
using pizzashop_Repository.ViewModel;
using pizzashop_Repository.Models;
using Microsoft.EntityFrameworkCore.Storage;
using static Auth_middleware;
using pizzashop_Services.ImplementationService;


namespace pizzashop.Controllers
{
    public class UserController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly IUser_Service user_Service;

        public UserController(IConfiguration configuration, IUser_Service userService, PizzashopContext context, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            user_Service = userService;
        }

        [CustomAuthorize("Dashboard", true)]
        public IActionResult Dashboard()
        {
            return View();
        }
        [CustomAuthorize("Users", true)]
        [HttpGet]

        // public IActionResult UserList(string? searchString, int pageNumber = 1, int pageSize = 5, string? sortOrder = null)
        // {
        //     var users = _userService.GetUsers(searchString, pageNumber, pageSize, sortOrder);
        //     var totalUsers = _userService.GetTotalUserCount(searchString);

        //     var userListPage = new FilterPaginationDto<UserViewModel>();
        //     userListPage.SetPaginationData(users, totalUsers, pageNumber, pageSize, searchString);

        //     return View(userListPage);
        // }
        public IActionResult UserList(string? searchString, int pageNumber = 1, int pageSize = 5, string? sortOrder = null)
        {
            var users = user_Service.GetUsers(searchString, pageNumber, pageSize, sortOrder);
            var totalUsers = user_Service.GetTotalUserCount(searchString);

            var userListPage = new FilterPaginationDto<UserViewModel>();
            userListPage.SetPaginationData(users, totalUsers, pageNumber, pageSize, searchString);

            return View(userListPage);
        }

        [HttpGet]
        public IActionResult GetUsers(string? searchString, int pageNumber = 1, int pageSize = 5, string? sortOrder = null)
        {
            var users = user_Service.GetUsers(searchString, pageNumber, pageSize, sortOrder);
            var totalUsers = user_Service.GetTotalUserCount(searchString);

            var userListPage = new FilterPaginationDto<UserViewModel>();
            userListPage.SetPaginationData(users, totalUsers, pageNumber, pageSize, searchString);

            return Json(userListPage);
        }

        [HttpGet]
        public JsonResult getState(int countryId)
        {
            List<stateDto> states = user_Service.GetStates(countryId);
            return Json(new { success = true, data = states });
        }
        [HttpGet]
        public JsonResult getCity(int stateID)
        {
            List<cityDto> countries = user_Service.GetCities(stateID);
            return Json(new { success = true, data = countries });
        }
        [CustomAuthorize("Users", false, true, false)]
        public IActionResult AddNewUser()
        {
            List<countryDto> countries = user_Service.GetCountries();
            ViewBag.countries = countries;
            return View();
        }

        [CustomAuthorize("Users", false, true, false)]
        [HttpPost]
        public async Task<IActionResult> addNewUser(AddNewUserModel model)
        {
            if (ModelState.IsValid)
            {
                string currentUserEmail = Request.Cookies["Email"] ?? "";
                if (await user_Service.AddUser(model, currentUserEmail))
                {
                    TempData["Success"] = "User created successfully";
                    return RedirectToAction("UserList", "User");
                }
                return View("AddNewUser");
            }
            return View(model);
        }
        [CustomAuthorize("Users", false, false, true)]
        [HttpPost]
        public IActionResult DeleteUser(int id)  
        {
            user_Service.IsDelete(id);
            return Json(new { success = true, message = "User deleted successfully!" });
        }
        [CustomAuthorize("Users", true, false, false)]
        [HttpGet]
        public IActionResult Profile()
        {
            string email = Request.Cookies["Email"] ?? "";

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login");
            }
            List<countryDto> countries = user_Service.GetCountries();
            ViewBag.countries = countries;
            var model = user_Service.GetProfileByEmail(email);

            if (model == null)
            {
                model = new ProfileModel();
            }
            return View(model);
        }
        [CustomAuthorize("Users", false, true, false)]
        [HttpPost]
        public IActionResult UpdateProfile(ProfileModel model)
        {
            bool isUpdated = user_Service.UpdateUser(model);
            if (isUpdated)
            {
                TempData["Success"] = "Profile updated successfully!";
                return RedirectToAction("UserList");
            }
            else
            {
                TempData["Error"] = "Failed to update profile.";
                return RedirectToAction("Profile");
            }
        }
        [CustomAuthorize("Users", false, true, false)]
        public IActionResult EditUser(int id)
        {
            List<countryDto> countries = user_Service.GetCountries();
            ViewBag.countries = countries;
            if (user_Service.getUserById(id) != null)
            {
                return View(user_Service.getUserById(id));
            }
            TempData["ErrorMessage"] = "Failed to update profile.";
            return View();
        }
        [HttpPost]
        public IActionResult updateUser(AddNewUserModel model)
        {
            if (user_Service.UpdateUserDb(model))
            {
                TempData["Success"] = "User Update Successfull";
                return RedirectToAction("UserList");
            }
            TempData["Error"] = "something went wrong";
            return RedirectToAction("UserList");
        }

        public IActionResult Cancel()
        {
            return RedirectToAction("Profile");
        }
    }
}

