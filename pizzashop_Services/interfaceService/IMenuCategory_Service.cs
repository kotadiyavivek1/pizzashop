using Microsoft.Extensions.Configuration;
using pizzashop_Repository.Models;
using pizzashop_Repository.ViewModel;

namespace pizzashop_Services.interfaceService;

public interface IMenuCategory_Service
{
    public CategoryModifierDto Getcategories();
    public List<Unit> GetallUnits();
    bool addCategory(AddCategory  model,string email);
    CategoryModifierDto getItemsByCategory(int categoryId);
    bool isDeletedItems(int itemId,string Email);
    CategoryModifierDto getAllMenu();
    bool addItem(AddItems model,string currentUserEmail);
    bool deleteCategory(int categoryId,string Email);
    bool updatemenuCategory(string email,Category model,int id);
    bool DeleteSelectedItems(List<int> itemsId,string email);
    List<ModifiersDto> getModifiersbyId(int id);
    bool AddNewModfier(List<int> modifiersid,string email,addNewModifier model);
    List<Category> GetallCategories();
    List<MenuItems> menuItemsList(int Categoryid);
    public List<ModifiersDto> GetAllModifiers(int groupId);
    public List<ModifierGroupDto> GetAllModifierGroup();
    public MenuItems getItemDetails(int id);
    public ModifierGroupDto GetModifierGroup(int id);
    public bool isExistingItem(MenuItems model,string email);
    public FilterPaginationDto<CategoryModifierDto> filterPaginationItems(int categoryId,int pageNumber, int pageSize, string searchString);
    public FilterPaginationDto<ModifiersDto> GetAllModifiers(int pageNumber = 1, int pageSize = 5, string searchString = "");
}
