using pizzashop_Repository.Models;
using pizzashop_Repository.ViewModel;
namespace pizzashop_Repository.Interface;
public interface IMenuCategory_Repository
{
    CategoryModifierDto Getcategories();
    bool saveChanges(AddCategory model,string Email);
    List<MenuItems> GetMenuitems(int categoryId);
    bool isDeletedItem(int itemId,string Email);
    List<ModifierGroupDto> GetModifiergroups();
    List<Category> CategoriesbyList();
    bool SaveItem(AddItems model,string currentUserEmail,string profilePicturePath);
    bool isDeletedItemCategory(int categoryId,string email);
    bool updateCategory(string email,Category model,int id);
    bool isDeletedItems(List<int> itemsId,string email);
    List<ModifiersDto> GetModifiersbyId(int id);
    List<ModifiersDto> getAllModifiers();
    FilterPaginationDto<ModifiersDto> GetAllModifiers(int pageNumber = 1, int pageSize = 5, string searchString = "");
    bool addNewModifier(List<int> modifiersID, string email, addNewModifier model);
    // bool addNewModifier();
    List<Category> GetAlLcategories();
     public List<Unit> GetUnits();
    public List<ModifiersDto> GetAllModifiers(int groupId);
    public List<ModifierGroupDto> GetAllModifierGroup();

    public MenuItems getItemDetails(int id);
    public ModifierGroupDto GetModifierGroupById(int id);
    public bool updateItem(MenuItems updatedModel,string email);
    public bool UpdateModifierMapping(List<ModifiergroupDto> updatedMappings, string email, int Menuitemid);
    public FilterPaginationDto<CategoryModifierDto> pagination(int categoryId,int pageNumber, int pageSize, string searchString);
    
}