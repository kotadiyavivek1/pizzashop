using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using pizzashop_Repository.Interface;
using pizzashop_Repository.Models;
using pizzashop_Repository.ViewModel;
using static Auth_middleware;

namespace pizzashop.Controllers
{

    public class RolePermissionController : Controller
    {
        private readonly PizzashopContext _db;
        private readonly IRole_Services _role;
        public RolePermissionController(IRole_Services role, PizzashopContext _db)
        {
            _role = role;
            this._db = _db;
        }


        [HttpGet]
        [CustomAuthorize("RolesAndPermissions", true)]
        public IActionResult Role()
        {
            List<RoleDto> roles = _role.GetAllRoles();
            return View(roles);
        }

        [HttpGet]
        [CustomAuthorize("RolesAndPermissions", true)]
        public IActionResult RolePermission(int roleId)
        {

            var permission = _role.GetRolePermissions(roleId);
            if (!permission.Any())
            {
                return NotFound("no permission Found for this role");
            }
            return View(permission);
        }

        [HttpPost("/RolePermission/UpdatePermissions")]
        public IActionResult UpdatePermissions([FromBody] List<UpdatePermissionDto> permissions)
        {
            if (permissions == null || !permissions.Any())
            {
                return BadRequest(new { message = "No permissions provided!" });
            }

            try
            {
                foreach (var permissionDto in permissions)
                {
                    var existingPermission = _db.Rolepermissions
                        .FirstOrDefault(rp => rp.Roleid == permissionDto.roleId && rp.Permissionid == permissionDto.PermissionId) ?? new Rolepermission();

                    switch (permissionDto.Type)
                    {
                        case "CanView":
                            existingPermission.Canview = permissionDto.IsChecked;
                            break;
                        case "CanEdit":
                            existingPermission.Canedit = permissionDto.IsChecked;
                            break;
                        case "CanDelete":
                            existingPermission.Candelete = permissionDto.IsChecked;
                            break;
                    }
                    _db.SaveChanges();
                    _db.Update(existingPermission);
                }
                return Ok(new { message = "Permissions updated successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating permissions.", error = ex.Message });
            }
        }

    }
}
