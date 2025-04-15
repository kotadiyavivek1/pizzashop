using System;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using pizzashop_Repository.Implementation;
using pizzashop_Repository.Interface;
using pizzashop_Repository.Models;
using pizzashop_Repository.ViewModel;
using pizzashop_Services.interfaceService;
namespace pizzashop_Repository.ImplementationService{
    public class MenuCategory_Service : IMenuCategory_Service
    {

        private readonly IMenuCategory_Repository menuCategoryRepository;
        public MenuCategory_Service(IMenuCategory_Repository _menuCategoryRepository){
            menuCategoryRepository=_menuCategoryRepository; 
        }
        public List<Unit> GetallUnits()
        {
            return menuCategoryRepository.GetUnits();
        }
        public bool addCategory(AddCategory model,string email)
        {
            if(model.CategoryName!=null)
            {
                if(menuCategoryRepository.saveChanges(model,email)){
                    return true;
                }
            }
            return false;
        }

        

        public bool addItem(AddItems model,string currentUserEmail)
        {
           string profilePicturePath = "/images/default-profile.png";

            if (model.Image != null && model.Image.Length > 0)
            {
                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadFolder);
                string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.Image.FileName)}";
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                model.Image.CopyToAsync(stream);
                profilePicturePath = $"~/uploads/{uniqueFileName}";
            }

            if(menuCategoryRepository.SaveItem(model,currentUserEmail,profilePicturePath)){
                return true;
            }
            else
            {
                return false;
            }
        }
        // return directly  llike menuCategoryRepository.addNewModifier(modifiersid,email,model)
        public bool AddNewModfier(List<int> modifiersid, string email, addNewModifier model)
        {
             if(menuCategoryRepository.addNewModifier(modifiersid,email,model)){
                    return true;
            }
            return false;
        }


        public bool deleteCategory(int categoryId,string Email)
        {
            if(menuCategoryRepository.isDeletedItemCategory(categoryId,Email))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteSelectedItems(List<int> itemsId,string Email)
        {
            if(menuCategoryRepository.isDeletedItems(itemsId,Email))
            {
                return true;
            }
            return false;
        }

        public bool IsUpdate(MenuItems updatedModel,string email)
        {
            return menuCategoryRepository.updateItem(updatedModel,email);
        }

        public List<Category> GetallCategories()
        {
         return menuCategoryRepository.GetAlLcategories();
        }
        public List<ModifierGroupDto> GetAllModifierGroup()
        {
            return menuCategoryRepository.GetAllModifierGroup();
        }
        public List<ModifiersDto> GetAllModifiers(int groupId)
        {
            return menuCategoryRepository.GetAllModifiers(groupId);
        }

        public CategoryModifierDto getAllMenu()
        {
             CategoryModifierDto model = new CategoryModifierDto(){
                Categories=menuCategoryRepository.CategoriesbyList(),
                modifierGroups = menuCategoryRepository.GetModifiergroups(),
                GetAllModifiers = menuCategoryRepository.getAllModifiers()
             };
             return model;
        }


        public CategoryModifierDto Getcategories()
        {
            var categories = menuCategoryRepository.Getcategories();
            return categories;
        }

        public List<ModifiersDto> getModifiersbyId(int id)
        {
           List<ModifiersDto> modifiers = menuCategoryRepository.GetModifiersbyId(id);
           return modifiers;
        }

        public List<MenuItems> menuItemsList(int Categoryid)
        {
            return menuCategoryRepository.GetMenuitems(Categoryid);
        }


        public bool updatemenuCategory(string email, Category model,int id)
        {
            if(menuCategoryRepository.updateCategory(email,model,id))
            {
                return true;
            }
            return false;
        }


        CategoryModifierDto IMenuCategory_Service.getItemsByCategory(int categoryId)
        {
            List<MenuItems> items = menuCategoryRepository.GetMenuitems(categoryId);
            CategoryModifierDto model = new(){
                menuItems = items,
            };
            return model;
        }
        
        bool IMenuCategory_Service.isDeletedItems(int itemId,string Email)
        {
            bool IsDeletedItemsDb = menuCategoryRepository.isDeletedItem(itemId,Email);
            if(IsDeletedItemsDb==true){
                return true;
            } 
            else{
                return false;
            }
        }

        public MenuItems getItemDetails(int id)
        {
           MenuItems items = menuCategoryRepository.getItemDetails(id);
           return items;
        }

        public ModifierGroupDto GetModifierGroup(int id)
        {
            ModifierGroupDto modifierGroup = menuCategoryRepository.GetModifierGroupById(id);
           return modifierGroup; 
        }

        public bool isExistingItem(MenuItems model, string email)
        {
            if(menuCategoryRepository.updateItem(model,email))
            {
                return true;
            }
            return false;
        }

        public FilterPaginationDto<CategoryModifierDto> filterPaginationItems(int categoryId, int pageNumber, int pageSize, string searchString)
        {
            return menuCategoryRepository.pagination(categoryId, pageNumber, pageSize, searchString);
        }
        
        public FilterPaginationDto<ModifiersDto> GetAllModifiers(int pageNumber = 1, int pageSize = 5, string searchString = ""){
            return menuCategoryRepository.GetAllModifiers(pageNumber,pageSize,searchString);
        }
    }
}