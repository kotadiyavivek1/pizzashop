using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using pizzashop_Repository.Models;
using pizzashop_Repository.ViewModel;
using pizzashop_Repository.Interface;
using System.Reflection.Metadata.Ecma335;


namespace pizzashop_Repository.Implementation;
public class User_Repository : IUser_Repository
{
    private readonly PizzashopContext _context;
    public User_Repository(PizzashopContext context)
    {
        _context = context;
    }
    public void AddUser(User user)
    {
        _context.Users.Add(user);
    }
    public User GetUserById(int userId)
    {
        return _context.Users.FirstOrDefault(x => x.Id == userId)!;
    }
    public void SaveChanges()
    {
        _context.SaveChanges();
    }
    public IQueryable<User> GetUsers(string searchString, string sortOrder, int page, int pageSize, out int totalRecords)
    {
        var users = _context.Users.Include(u => u.Role).AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            users = users.Where(u => (u.Firstname + " " + u.Lastname).Contains(searchString));
        }

        users = sortOrder switch
        {
            "name_asc" => users.OrderBy(u => u.Firstname + " " + u.Lastname),
            "name_desc" => users.OrderByDescending(u => u.Firstname + " " + u.Lastname),
            "role_asc" => users.OrderBy(u => u.Role.Name),
            "role_desc" => users.OrderByDescending(u => u.Role.Name),
            _ => users
        };
        totalRecords = users.Count();
        return users.Where(u => u.Isdeleted == false).Skip((page - 1) * pageSize).Take(pageSize);
    }
    public Task<User> getUpdateUserByEmail(string email)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user == null)
        {
            return Task.FromResult<User>(null);
        }
        return Task.FromResult(user);
    }
    public bool IsDeleteDb(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            user.Isdeleted = true;
            _context.Update(user);
            _context.SaveChanges();
            return true;
        }
        else
        {
            return false;
        }
    }
    public ProfileModel GetUserByEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return null;
        }

        var user = _context.Users
            .Include(u => u.Role)
            .FirstOrDefault(u => u.Email == email);
        if (user != null)
        {
            ProfileModel model = new()
            {
                Id = user.Id,
                FirstName = user.Firstname ?? "",
                LastName = user.Lastname ?? "",
                Role = user.Role.Name,
                Email = user.Email,
                UserName = user.Username,
                Phone = user.Phone ?? "",
                Address = user.Address ?? "",
                Zipcode = user.Zipcode ?? "",
                profileImage = user.Profileimage ?? "",
                StateID = user.Stateid,
                CountryID = user.Countryid,
                CityID = user.Cityid
            };
            return model;
        }
        return new ProfileModel();
    }

    public User GetUserDtoByEmail(string email)
    {
        User user = _context.Users.FirstOrDefault(u => u.Email == email) ?? new User();
        return user;
    }


    public bool UpdateUser(ProfileModel model, string profilePicturePath)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
        if (user == null)
            return false;
        user.Firstname = model.FirstName;
        user.Lastname = model.LastName;
        user.Username = model.UserName ?? "";
        user.Phone = model.Phone;
        user.Address = model.Address;
        user.Zipcode = model.Zipcode;
        user.Profileimage = profilePicturePath;
        user.Cityid = model.CityID;
        user.Countryid = model.CountryID;
        user.Stateid = model.StateID;
        _context.Users.Update(user);
        _context.SaveChanges();
        return true;
    }

    public List<countryDto> GetAllCountries()
    {
        return _context.Countries.Select(
            c => new countryDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
    }

    public List<stateDto> GetAllStates(int countryid)
    {
        return _context.States.Where(c => c.Countryid == countryid).Select(
            c => new stateDto
            {
                Id = c.Id,
                CountryId = c.Countryid,
                Name = c.Name,
            }
        ).ToList();
    }

    public List<cityDto> GetAllCities(int stateid)
    {
        return _context.Cities.Where(c => c.Stateid == stateid).Select(
            c => new cityDto
            {
                Id = c.Id,
                StateId = c.Stateid,
                Name = c.Name,
            }).ToList();
    }

    public AddNewUserModel GetUserDetail(int userid)
    {
        var user = _context.Users.FirstOrDefault(c => c.Id == userid);
        if (user != null)
        {
            AddNewUserModel model = new()
            {
                FirstName = user.Firstname,
                LastName = user.Lastname,
                UserName = user.Lastname,
                roleID = user.Roleid,
                Email = user.Email,
                Password = user.Password,
                CountryID = user.Countryid,
                StateID = user.Stateid,
                CityID = user.Cityid,
                ZipCode = user.Zipcode,
                Address = user.Address,
                Phone = user.Phone,
                ProfilePicturePath = user.Profileimage,
                Status = user.Isactive
            };
            return model;
        }
        else
        {
            return new AddNewUserModel();
        }

    }

    public bool updateUser(AddNewUserModel model, string profilePicturePath)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
        if (user != null)
        {
            user.Modifiedby = user.Id;
            user.Firstname = model.FirstName;
            user.Lastname = model.LastName;
            user.Profileimage = profilePicturePath;
            user.Address = model.Address;
            user.Zipcode = model.ZipCode;
            user.Countryid = model.CountryID;
            user.Cityid = model.CityID;
            user.Isactive = model.Status;
            user.Modifiedat = DateTime.Now;
            _context.SaveChanges();
            _context.Update(user);
            return true;
        }
        return false;
    }

    public List<UserViewModel> GetUsers(string? searchString, int pageNumber, int pageSize, string? sortOrder)
    {
        var query = _context.Users.Include(u => u.Role).Where(u=>u.Isdeleted==false).AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(u => (u.Firstname + " " + u.Lastname).Contains(searchString));
        }

        query = sortOrder switch
        {
            "name_asc" => query.OrderBy(u => u.Firstname).ThenBy(u => u.Lastname),
            "name_desc" => query.OrderByDescending(u => u.Firstname).ThenByDescending(u => u.Lastname),
            "role_asc" => query.OrderBy(u => u.Role.Name),
            "role_desc" => query.OrderByDescending(u => u.Role.Name),
            _ => query.OrderBy(u => u.Id),
        };

        return query.Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(u => new UserViewModel
                    {
                        Id = u.Id,
                        FullName = u.Firstname + " " + u.Lastname,
                        Email = u.Email,
                        Phone = u.Phone ?? "",
                        RoleName = u.Role.Name,
                        Status = u.Isactive ?? true,
                        profileImage = u.Profileimage ?? "",
                    })
                    .ToList();
    }

    public int GetTotalUserCount(string? searchString)
    {
       var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(u => (u.Firstname + " " + u.Lastname).Contains(searchString));
            }

            return query.Count();
    }




}