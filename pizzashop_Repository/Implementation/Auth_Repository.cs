    using System.Linq.Expressions;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using pizzashop_Repository.Interface;
using pizzashop_Repository.Models;
using pizzashop_Repository.ViewModel;

namespace pizzashop_Repository.Implementation;

public class Auth_Repository:IAuth_Repository
{
    private readonly PizzashopContext _context;

    public Auth_Repository(PizzashopContext context)
    {
        _context = context;
    }

    public bool VerifyUser(string email, string password)
    {
        var checkEmail = _context.Users.FirstOrDefault(u => u.Email == email && u.Isdeleted==false);
        bool isPasswordValid = false;
        if(checkEmail!=null){
            isPasswordValid = BCrypt.Net.BCrypt.Verify(password, checkEmail.Password);
        }
        if(checkEmail != null && isPasswordValid == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public string getRoleName(int roleId)
    {
        var role = _context.Roles.FirstOrDefault(r => r.Id == roleId);
        string roleName = role != null ? role.Name : string.Empty;
        return roleName;
    }

    public int getRoleId(string email)
    {
        var roleId= _context.Users.FirstOrDefault(u => u.Email == email);   
        int roleID = roleId != null ? roleId.Roleid : 0;
        return roleID;
    }

    public bool VerifyUserEmail(string email)
    {
        var checkEmail = _context.Users.FirstOrDefault(u => u.Email == email);
        if(checkEmail != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    async Task IAuth_Repository.updatePassword(string email, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user != null)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(password);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> changePassword(string currentPassword, string NewPassword,string email)
    {
        var user = _context.Users.FirstOrDefault(u=>u.Email==email);
        if(user!=null){
            user.Password=BCrypt.Net.BCrypt.HashPassword(NewPassword);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public ProfileModel ViewDataUser(string email)
    {
            var user = _context.Users.FirstOrDefault(u=>u.Email==email);
            if(user!=null){         
                ProfileModel model1 = new()
                {
                    FirstName = user.Firstname, 
                    LastName = user.Lastname,
                    Address =user.Address,
                    Phone = user.Phone,
                    UserName = user.Username,
                    Email =user.Email,
                    profileImage=user.Profileimage,
                    Role = _context.Roles.FirstOrDefault(u => u.Id == user.Roleid)?.Name ?? string.Empty
                };
            return model1;
            }
            ProfileModel  model= new ()
            {
                FirstName = "Guest",
                LastName = "User",
                profileImage = "/images/default-profile.png"
            };
            return model;
    }

    public void saveToken(string token,string Email)
    {
        var user=_context.Users.FirstOrDefault(u=>u.Email==Email)?? new User();
        try
        {
            if(!string.IsNullOrEmpty(token))
            {
                user.ResetPasswordToken=token;
                _context.SaveChanges();
                _context.Update(user);
            }
        }
        catch(Exception)
        {
            return;
        }
    }

    public bool VerifyResetToken(string Token,string email)
    {
        var user = _context.Users.FirstOrDefault(u=>u.ResetPasswordToken==Token);
        if(user!=null)
        {
            return true;
        }   
        return false;
    }

}