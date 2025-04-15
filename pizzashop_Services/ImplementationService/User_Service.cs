using System.Security.Cryptography.X509Certificates;
using pizzashop_Repository.Models;
using Microsoft.AspNetCore.Mvc;
using pizzashop_Repository.Implementation;
using pizzashop_Repository.Interface;
using pizzashop_Repository.ViewModel;
using pizzashop_Services.interfaceService;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Pqc.Crypto.Falcon;

namespace pizzashop_Services.ImplementationService
{

    public class User_Service : IUser_Service
    {
        private readonly IAuth_Repository _Repository;
        private readonly IEmail_Service _email_Service;
        private readonly IUser_Repository _user_Repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public User_Service(IAuth_Repository Repository, IUser_Repository user_Repository, IEmail_Service email_Service, IHttpContextAccessor httpContextAccessor)
        {
            _Repository = Repository;
            _user_Repository = user_Repository;
            _email_Service = email_Service;
            _httpContextAccessor = httpContextAccessor;
        }

        public ProfileModel profileView(string Email)
        {
            ProfileModel userData = _Repository.ViewDataUser(Email);
            return userData;
        }

        public ProfileModel GetProfileByEmail(string email)
        {
            return _user_Repository.GetUserByEmail(email);
        }
        [HttpGet]
        public IEnumerable<UserViewModel> GetUsers(string searchString, string sortOrder, int pageNumber, int pageSize, out int totalRecords)
        {
            var users = _user_Repository.GetUsers(searchString, sortOrder, pageNumber, pageSize, out totalRecords);
            return users.Select(u => new UserViewModel
            {
                Id = u.Id,
                FullName = u.Firstname + " " + u.Lastname,
                Email = u.Email,
                Phone = u.Phone ?? "",
                RoleName = u.Role.Name,
                Status = u.Isactive ?? true,
                profileImage = u.Profileimage ?? ""
            }).ToList();
        }
        
        public async Task<bool> AddUser(AddNewUserModel model, string currentUserEmail)
        {
            var currentUser = _user_Repository.GetUserByEmail(currentUserEmail);
            if (currentUser == null) return false;

            var existingUser = _user_Repository.GetUserDtoByEmail(model.Email);
            if (existingUser != null) return false;

            string profilePicturePath = "/images/default-profile.png";

            if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
            {
                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadFolder);
                string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.ProfilePicture.FileName)}";
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                await using var stream = new FileStream(filePath, FileMode.Create);
                await model.ProfilePicture.CopyToAsync(stream);
                profilePicturePath = $"~/uploads/{uniqueFileName}";
            }
            var newUser = new User
            {
                Firstname = model.FirstName,
                Lastname = model.LastName,
                Username = string.IsNullOrWhiteSpace(model.UserName) ? "Admin@123" : model.UserName,
                Email = model.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(string.IsNullOrWhiteSpace(model.Password) ? "Admin@123" : model.Password),
                Phone = model.Phone,
                Roleid = model.roleID,
                Countryid = model.CountryID,
                Stateid = model.StateID,
                Cityid = model.CityID,
                Zipcode = model.ZipCode,
                Address = model.Address,
                Createdby = currentUser.Id,
                Createdat = DateTime.Now, 
                Profileimage = profilePicturePath
            };

            // Send Email Notification
            string loginUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/Auth/Login";
            string subject = "Your PizzaShop Login Credentials";
            string body = $@"
            <h2>Welcome to PizzaShop!</h2>
            <p>Here are your login details:</p>
            <ul>
                <li><strong>Email:</strong> {newUser.Email}</li>
                <li><strong>Password:</strong> {model.Password}</li>
            </ul>
                <p>Please log in and change your password for security reasons.</p>
                <p><a href='{loginUrl}'>Login</a></p>
            <br>
        <p>Best Regards,<br><strong>PizzaShop Team</strong></p>";
            try
            {
                await _email_Service.SendEmailAsync(newUser.Email, subject, body);
            }
            catch (Exception)
            {
                return false;
            }
            _user_Repository.AddUser(newUser);
            _user_Repository.SaveChanges();
            return true;
        }

        public bool UpdateUser(ProfileModel model)
        {
            string profilePicturePath = model.profileImage; // Keep existing image path

            if (model.ProfileImageFile != null && model.ProfileImageFile.Length > 0)
            {
                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadFolder);

                string uniqueFileName = $"{Guid.NewGuid()}_{model.ProfileImageFile.FileName}";
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfileImageFile.CopyTo(stream);
                }

               profilePicturePath = $"/uploads/{uniqueFileName}"; // Save relative path
            }

            return _user_Repository.UpdateUser(model, profilePicturePath);
        }



        public bool IsDelete(int id)
        {
            if (_user_Repository.IsDeleteDb(id))
            {
                return true;
            }
            return false;
        }

        public List<countryDto> GetCountries()
        {
           List<countryDto> countries = _user_Repository.GetAllCountries();
           return countries;
        }

        public List<stateDto> GetStates(int countryId)
        {
            List<stateDto> states = _user_Repository.GetAllStates(countryId);
            return states;
        }

        public List<cityDto> GetCities(int stateID)
        {
            return _user_Repository.GetAllCities(stateID);
        }

        public AddNewUserModel getUserById(int id)
        {
            return _user_Repository.GetUserDetail(id);
        }

         public bool UpdateUserDb(AddNewUserModel model)
        {
            if (model.Email != null)
            {
                string profilePicturePath = model.ProfilePicturePath ?? "";
                if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
                {
                    string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfilePicture.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.ProfilePicture.CopyTo(fileStream);
                    }
                    profilePicturePath = $"/uploads/{uniqueFileName}";
                }

                if (_user_Repository.updateUser(model, profilePicturePath))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

       public List<UserViewModel> GetUsers(string? searchString, int pageNumber, int pageSize, string? sortOrder)
        {
            return _user_Repository.GetUsers(searchString, pageNumber, pageSize, sortOrder);
        }

        public int GetTotalUserCount(string? searchString)
        {
            return _user_Repository.GetTotalUserCount(searchString);
        }

    }
}