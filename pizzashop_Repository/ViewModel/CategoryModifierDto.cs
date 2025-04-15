
using Microsoft.EntityFrameworkCore.Storage;

namespace pizzashop_Repository.ViewModel;
public class CategoryModifierDto
{   
    public AddCategory? NewCategory { get; set; } = new AddCategory(); 
    public List<Category> ? Categories {get; set;} 

    public List<MenuItems> ? menuItems{get; set;}  
    public MenuItems? menuItem{get; set;}
    public AddItems addItems{get; set;} = new AddItems();
    public  List<ModifierGroupDto> ? modifierGroups{get; set;}
    public Category? category{get; set;}
    public List<ModifiersDto>? modifiers{get; set;}
    public List<ModifiersDto>? GetAllModifiers{get; set;}
   
       public  addNewModifier? addNewModifier{get; set;} =new addNewModifier();
}   

 

