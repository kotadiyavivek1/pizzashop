using Microsoft.AspNetCore.Mvc;
using pizzashop_Repository.Implementation;
using pizzashop_Repository.Models;
using pizzashop_Repository.ViewModel;
using pizzashop_Services.interfaceService;
using static Auth_middleware;

namespace pizzashop.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMenuCategory_Service menuCategory_Service;

        public MenuController(IMenuCategory_Service _menuCategory_Service)
        {
            menuCategory_Service = _menuCategory_Service;
        }

        [HttpPost]
        public IActionResult UpdateItem(MenuItems model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string email = Request.Cookies["Email"] ?? "";
                    bool updateResult = menuCategory_Service.isExistingItem(model, email);

                    return Json(new
                    {
                        success = updateResult,
                        message = updateResult ? "Item updated successfully." : "Something went wrong while updating the item."
                    });
                }

                // Collect validation errors
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new
                {
                    success = false,
                    message = string.Join(" | ", errorMessages)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "An error occurred: " + ex.Message
                });
            }
        }

        [HttpGet]
        public IActionResult ItemModifier()
        {
            CategoryModifierDto categories = menuCategory_Service.getAllMenu();
            return View(categories);
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = menuCategory_Service.GetallCategories();
            return Json(categories);
        }
        [HttpGet]
        public IActionResult GetUnits()
        {
            var units = menuCategory_Service.GetallUnits();
            return Json(units);
        }
        [HttpGet]
        public IActionResult GetCategorySection()
        {
            CategoryModifierDto categories = menuCategory_Service.Getcategories();
            return PartialView("_CategorySection", categories);
        }


        [HttpPost("/AddCategory/Menu")]
        public IActionResult AddCategory(AddCategory model)
        {
            if (ModelState.IsValid)
            {
                string currentUserEmail = Request.Cookies["Email"] ?? "";
                if (menuCategory_Service.addCategory(model, currentUserEmail))
                {
                    TempData["Success"] = "Category added";
                }
                else
                {
                    TempData["Error"] = "Category already added";
                }
            }
            TempData["Error"] = "Something went wrong";
            return RedirectToAction("ItemModifier");
        }

        [HttpGet]
        public IActionResult GetItemsByCategory(int CategoryId)
        {
            List<MenuItems> menuItems = menuCategory_Service.menuItemsList(CategoryId);
            CategoryModifierDto model = new CategoryModifierDto();
            model.menuItems = menuItems;
            return PartialView("_ItemsPartial", model);
        }

        // [HttpGet]
        // public IActionResult GetITemsByCategoryId(int CategoryId,int pageNumber,int pageSize,string searchString,int totalItems)
        // {
        //     List<MenuItems> menuItems = menuCategory_Service.menuItemsFilter(CategoryId, pageNumber, pageSize, searchString, totalItems);
        //     return Json(menuItems);
        // }

        [HttpGet]
        public IActionResult GetFilterItems(int categoryId, int pageNumber = 1, int pageSize = 3, string searchString = "")
        {
            FilterPaginationDto<CategoryModifierDto> filterItems = menuCategory_Service.filterPaginationItems(categoryId, pageNumber, pageSize, searchString);
            return PartialView("_ItemsPartial", filterItems);
        }


        [HttpPost]
        public JsonResult DeleteCategory(int id)
        {
            string Email = Request.Cookies["Email"] ?? "";
            bool result = menuCategory_Service.deleteCategory(id, Email);

            if (result)
            {
                return Json(new { success = true, message = "Category removed successfully!" });
            }
            else
            {
                return Json(new { success = false, message = "Something went wrong. Please try again." });
            }
        }


        [HttpPost]
        [Route("/Menu/DeleteItem")]
        public JsonResult DeleteItem(int itemId)
        {
            string Email = Request.Cookies["Email"] ?? "";
            if (menuCategory_Service.isDeletedItems(itemId, Email))
            {
                return Json(new { success = true, message = "Deleted successfully" });
            }
            else
            {
                return Json(new { success = false, message = "Something went wrong" });
            }
        }



        [HttpPost]
        [Route("Menu/AddItems")]
        public IActionResult AddItems([FromForm] AddItems model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new { success = false, errors = errorMessages });
            }

            string currentUserEmail = Request.Cookies["Email"] ?? "";
            bool isAdded = menuCategory_Service.addItem(model, currentUserEmail);

            return Json(new
            {
                success = isAdded,
                message = isAdded ? "New item added successfully" : "Something went wrong"
            });
        }

        [CustomAuthorize("Menu", false, true, false)]

        [HttpPost]
        public IActionResult EditCategory(Category model, int id)
        {


            string email = Request.Cookies["Email"] ?? "";
            if (menuCategory_Service.updatemenuCategory(email, model, id))
            {
                return RedirectToAction("ItemModifier");
            }
            return RedirectToAction("ItemModifier");
        }

        [HttpPost]
        public IActionResult DeleteSelectedItems([FromBody] List<int> itemsId)
        {
            string Email = Request.Cookies["Email"] ?? "";
            bool result = menuCategory_Service.DeleteSelectedItems(itemsId, Email);

            return Json(new
            {
                success = result,
                message = result ? "Items Deleted Successfully" : "Something went wrong"
            });
        }
        [HttpGet]
        public IActionResult GetModifiers(int id)
        {
            List<ModifiersDto> modifiersDtos = menuCategory_Service.getModifiersbyId(id);
            return PartialView("_ModifiersPartial", modifiersDtos);
        }

        [HttpPost]
        public IActionResult AddNewModifier([FromBody] addNewModifier model)
        {
            string email = Request.Cookies["email"] ?? "";
            if (email != null && model != null)
            {
                if (menuCategory_Service.AddNewModfier(model.SelectedModifiers ?? new List<int>(), email, model))
                {
                    TempData["Success"] = "Add New Modifier Successfully";
                    return RedirectToAction("ItemModifier");
                }
                TempData["Error"] = "Something went wrong";
                return RedirectToAction("ItemModifier");
            }
            return RedirectToAction("ItemModifier");
        }

        [HttpGet]
        [Route("/Menu/GetModifiersByGroup")]
        public IActionResult GetModifiersByGroup(int groupId)
        {
            List<ModifiersDto> modifiers = menuCategory_Service.GetAllModifiers(groupId);
            return Json(modifiers);
        }

        [HttpGet]
        [Route("/Menu/GetModifierGroups")]
        public IActionResult GetModifierGroups()
        {
            List<ModifierGroupDto> modifierGroups = menuCategory_Service.GetAllModifierGroup();
            return Json(modifierGroups);
        }

        [HttpGet]
        public IActionResult GetModifierGroupDetails(int groupId)
        {
            ModifierGroupDto modifierGroupDto = menuCategory_Service.GetModifierGroup(groupId);
            return Json(modifierGroupDto);
        }

        [HttpGet]
        [Route("/Menu/GetItemDetails")]
        public IActionResult GetItemDetails(int id)
        {
            MenuItems items = menuCategory_Service.getItemDetails(id);
            return Json(items);
        }


    }

}
