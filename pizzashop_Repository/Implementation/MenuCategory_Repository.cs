using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using pizzashop_Repository.Interface;
using pizzashop_Repository.Models;
using pizzashop_Repository.ViewModel;
namespace pizzashop_Repository.Implementation;
public class MenuCategory_Repository : IMenuCategory_Repository
{
    private readonly PizzashopContext _db;
    public MenuCategory_Repository(PizzashopContext db)
    {
        _db = db;
    }

    public List<Unit> GetUnits()
    {
        return _db.Units.ToList();
    }

    public List<Category> CategoriesbyList()
    {
        return _db.Menucategories.Where(i => i.Isdeleted == false).Select(i => new Category()
        {
            Categoryid = i.Id,
            CategoryName = i.Name,
            Description = i.Description,
        }).ToList();
    }

    public List<ModifierGroupDto> GetAllModifierGroup()
    {
        return _db.Modifiergroups.Select(
            i => new ModifierGroupDto()
            {
                ModifiergroupName = i.Name,
                id = i.Id
            }
        ).ToList();
    }

    public List<ModifiersDto> GetAllModifiers(int groupId)
    {
        return _db.Modifiergroupmodifiers.Where(i => i.Modifiergroupid == groupId).Include(i => i.Modifier).Select(i => new ModifiersDto()
        {
            id = i.Modifier.Id,
            name = i.Modifier.Name,
            rate = i.Modifier.Rate
        }).ToList();
    }


    public List<Category> GetAlLcategories()
    {
        var category = _db.Menucategories.Where(m => m.Isdeleted == false).Select(m => new Category
        {
            CategoryName = m.Name,
            Description = m.Description,
            Categoryid = m.Id
        }).ToList();
        return category;
    }
    public CategoryModifierDto Getcategories()
    {
        var category = _db.Menucategories.Where(m => m.Isdeleted == false).Select(m => new Category
        {
            CategoryName = m.Name,
            Description = m.Description,
            Categoryid = m.Id
        }).ToList();

        if (category == null)
        {
            CategoryModifierDto categories = new()
            {
                Categories = new List<Category>()
            };
        }
        CategoryModifierDto model = new()
        {
            Categories = category,
        };
        return model;
    }

    public List<MenuItems> GetMenuitems(int categoryId)
    {
        string defaultImagepath =
        "/images/dining-menu.png";
        var menuitem = _db.Menuitems.Include(i => i.Mappingmenuitemwithmodifiers).Include(u => u.Unit).Where(i => i.Categoryid == categoryId && i.Isdeleted == false)
        .Select(i => new MenuItems()
        {
            CategoryId = i.Categoryid,
            itemId = i.Id,
            Type = i.Type ?? true,
            UnitId = i.Unitid ?? 0,
            Name = i.Name,
            Price = i.Rate,
            Quantity = i.Quantity ?? 0,
            IsAvailable = i.Isavailable ?? true,
            ImagePath = string.IsNullOrEmpty(i.Image) ? defaultImagepath : i.Image,
            UnitName = i.Unit.Name,
            IsDefaultTax = i.Isdefaulttax ?? true,
            Isfavourite = i.Isfavourite ?? false,
            ShortCode = i.Shortcode,
            Description = i.Description,
            Modifiergroup = i.Mappingmenuitemwithmodifiers.Select(mi => new ModifiergroupDto
            {
                Name = mi.Modifiergroup.Name,
                MinSelection = mi.Minselectionrequired ?? 0,
                MaxSelection = mi.Maxselectionallowed ?? 0,
                ModifierGroupId = mi.Modifiergroupid
            }).ToList()
        }).ToList();
        return menuitem;
    }


    public List<ModifierGroupDto> GetModifiergroups()
    {
        List<ModifierGroupDto> modifierGroups = _db.Modifiergroups.Where(i => i.Isdeleted == false).Select(i => new ModifierGroupDto()
        {
            id = i.Id,
            ModifiergroupName = i.Name
        }).ToList();
        return modifierGroups;
    }

    public List<ModifiersDto> GetModifiersbyId(int modifierGroupId)
    {
        return _db.Modifiergroupmodifiers
            .Where(m => m.Modifiergroupid == modifierGroupId)

            .Select(m => new ModifiersDto
            {
                id = m.Modifier.Id,
                modifierGroupID = m.Modifiergroupid,
                name = m.Modifier.Name,
                rate = m.Modifier.Rate,
                quantity = m.Modifier.Quantity,
                description = m.Modifier.Description
            })
            .ToList();
    }



    public bool isDeletedItem(int itemId, string email)
    {
        var user = _db.Users.FirstOrDefault(u => u.Email == email);
        var items = _db.Menuitems.FirstOrDefault(i => i.Id == itemId);
        if (items != null && user != null)
        {
            items.Isdeleted = true;
            items.Modifiedby = user.Id;
            items.Modifiedat = DateTime.Today;
            _db.SaveChanges();
            _db.Update(items);
            return true;
        }
        return false;
    }

    public bool isDeletedItemCategory(int categoryId, string email)
    {
        var user = _db.Users.FirstOrDefault(u => u.Email == email);
        var category = _db.Menucategories
                              .Include(c => c.Menuitems)
                              .FirstOrDefault(c => c.Id == categoryId);
        if (category != null && user != null)
        {
            category.Isdeleted = true;
            category.Modifiedby = user.Id;

            foreach (var item in category.Menuitems)
            {
                item.Isdeleted = true;
            }

            _db.SaveChanges();
            _db.Update(category);
            return true;
        }
        return false;
    }

    public bool isDeletedItems(List<int> itemsId, string email)
    {
        var user = _db.Users.FirstOrDefault(u => u.Email == email);
        if (user != null && itemsId != null)
        {
            foreach (var item in itemsId)
            {
                var selectedItem = _db.Menuitems.FirstOrDefault(u => u.Id == item);
                if (selectedItem != null && user != null)
                {
                    selectedItem.Isdeleted = true;
                    selectedItem.Modifiedby = user.Id;
                    selectedItem.Modifiedat = DateTime.Today;
                    _db.Update(selectedItem);
                    _db.SaveChanges();
                }
            }
            return true;
        }
        return false;
    }


    public bool saveChanges(AddCategory model, string Email)
    {
        var user = _db.Users.FirstOrDefault(u => u.Email == Email);
        if (user != null)
        {
            var menu = new Menucategory
            {
                Name = model.CategoryName ?? "",
                Description = model.Description,
                Createdby = user.Id,
                Createdat = DateTime.Now
            };
            _db.Menucategories.Add(menu);
            _db.SaveChanges();
            return true;
        }
        return false;
    }
    public bool updateItem(MenuItems updatedModel, string email)
    {
        var item = _db.Menuitems.FirstOrDefault(i => i.Id == updatedModel.itemId);
        if (item != null && updatedModel != null)
        {
            item.Categoryid = updatedModel.CategoryId;
            item.Type = updatedModel.Type;
            item.Unitid = updatedModel.UnitId;
            item.Isfavourite = updatedModel.Isfavourite;
            item.Isavailable = updatedModel.IsAvailable;
            item.Name = updatedModel.Name;
            item.Rate = updatedModel.Price;
            item.Quantity = updatedModel.Quantity;
            item.Image = updatedModel.ImagePath;
            item.Description = updatedModel.Description;
            item.Taxpercentage = updatedModel.TaxPercentage;
            item.Shortcode = updatedModel.ShortCode;
            item.Isdefaulttax = updatedModel.IsDefaultTax;
            item.Modifiedat = DateTime.Now;
            item.Modifiedby = _db.Users.FirstOrDefault(u => u.Email == email)?.Id;
            _db.Menuitems.Update(item);
            _db.SaveChanges();
        }
        if (UpdateModifierMapping(updatedModel.Modifiergroup, email, updatedModel.itemId))
        {
            return true;
        }
        return false;

    }

    public bool UpdateModifierMapping(List<ModifiergroupDto> updatedMappings, string email, int Menuitemid)
    {
        if (updatedMappings == null || updatedMappings.Count == 0)
        {
            return false;
        }
        foreach (var mapping in updatedMappings)
        {

            var existingMapping = _db.Mappingmenuitemwithmodifiers.FirstOrDefault(m => m.Modifiergroupid == mapping.ModifierGroupId && m.Menuitemid == Menuitemid);
            if (existingMapping != null)
            {
                existingMapping.Minselectionrequired = mapping.MinSelection;
                existingMapping.Maxselectionallowed = mapping.MaxSelection;
                existingMapping.Modifiedat = DateTime.Now;
                existingMapping.Modifiedby = _db.Users.FirstOrDefault(u => u.Email == email)?.Id;
                _db.Mappingmenuitemwithmodifiers.Update(existingMapping);
                _db.Mappingmenuitemwithmodifiers.Update(existingMapping);
                _db.SaveChanges();
            }
            else
            {
                var newMapping = new Mappingmenuitemwithmodifier
                {
                    Menuitemid = Menuitemid,
                    Modifiergroupid = mapping.ModifierGroupId,
                    Minselectionrequired = mapping.MinSelection,
                    Maxselectionallowed = mapping.MaxSelection,
                    Createdby = _db.Users.FirstOrDefault(u => u.Email == email)?.Id ?? 0,
                    Createdat = DateTime.Now
                };
                _db.Mappingmenuitemwithmodifiers.Add(newMapping);
                _db.SaveChanges();
            }
        }
        return true;
    }



    public bool SaveItem(AddItems model, string currentUserEmail, string profileImage)
    {
        using var transaction = _db.Database.BeginTransaction();
        try
        {
            User user = _db.Users.FirstOrDefault(u => u.Email == currentUserEmail) ?? new User();
            var name = _db.Menuitems.FirstOrDefault(u => u.Name == model.Name);
            if (name != null)
            {
                return false;
            }
            Menuitem item = new()
            {
                Categoryid = model.CategoryId,
                Name = model.Name,
                Type = model.Type,
                Rate = model.Price,
                Quantity = model.Quantity,
                Image = model.ImagePath,
                Unitid = model.UnitId,
                Isavailable = model.IsAvailable,
                Description = model.Description,
                Taxpercentage = model.TaxPercentage,
                Shortcode = model.ShortCode,
                Isdefaulttax = model.IsDefaultTax,
                Createdby = user.Id
            };

            _db.Menuitems.Add(item);
            _db.SaveChanges();


            if (model.Modifiergroups != null && model.Modifiergroups.Any())
            {
                foreach (var group in model.Modifiergroups)
                {
                    var modifierGroup = new Mappingmenuitemwithmodifier
                    {
                        Menuitemid = item.Id,
                        Modifiergroupid = group.ModifierGroupId,
                        Minselectionrequired = group.MinSelection,
                        Maxselectionallowed = group.MaxSelection,
                        Createdby = user.Id
                    };

                    _db.Mappingmenuitemwithmodifiers.Add(modifierGroup);
                }
                _db.SaveChanges();
            }

            transaction.Commit();
            return true;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.WriteLine($"Error adding item: {ex.Message}");
            return false;
        }
    }


    public bool updateCategory(string email, Category model, int id)
    {
        var user = _db.Users.FirstOrDefault(u => u.Email == email);
        var category = _db.Menucategories.FirstOrDefault(m => m.Id == id);
        if (user != null && category != null)
        {
            category.Name = model.CategoryName ?? "";
            category.Description = model.Description;
            category.Modifiedby = user.Id;
            category.Modifiedat = DateTime.Today;
            _db.Menucategories.Update(category);
            _db.SaveChanges();
            return true;
        }
        return false;
    }

    public List<ModifiersDto> getAllModifiers()
    {
        var modifiersDtos = _db.Modifiers.Include(u => u.Unit).Select(u => new ModifiersDto
        {
            unit = u.Unit.Name,
            id = u.Id,
            modifierGroupID = u.Modifiergroupid,
            name = u.Name,
            rate = u.Rate,
            quantity = u.Quantity,
            description = u.Description
        }).ToList();
        return modifiersDtos;
    }



    public bool addNewModifier(List<int> modifiersID, string email, addNewModifier model)
    {
        var user = _db.Users.FirstOrDefault(u => u.Email == email);
        if (user != null)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var newModifierGroup = new Modifiergroup()
                    {
                        Name = model.ModifiergroupName ?? "",
                        Createdat = DateTime.Now,
                        Description = model.Description,
                        Createdby = user.Id,
                    };

                    _db.Modifiergroups.Add(newModifierGroup);
                    _db.SaveChanges();

                    if (modifiersID != null && modifiersID.Any())
                    {
                        foreach (var modifierId in modifiersID)
                        {
                            var modifierGroupModifier = new Modifiergroupmodifier()
                            {
                                Modifiergroupid = newModifierGroup.Id,
                                Modifierid = modifierId,
                                Createdby = user.Id,
                                Createdat = DateTime.Now
                            };

                            _db.Modifiergroupmodifiers.Add(modifierGroupModifier);
                        }
                    }
                    _db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        return false;
    }
    public MenuItems getItemDetails(int id)
    {
        var item = _db.Menuitems.Include(mg => mg.Mappingmenuitemwithmodifiers).ThenInclude(u => u.Modifiergroup).FirstOrDefault(u => u.Id == id);
        if (item != null)
        {
            MenuItems items = new()
            {
                itemId = item.Id,
                UnitId = item.Unitid ?? 0,
                Name = item.Name,
                CategoryId = item.Categoryid,
                Type = item.Type ?? true,
                Price = item.Rate,
                Quantity = item.Quantity ?? 0,
                IsAvailable = item.Isavailable ?? true,
                IsDefaultTax = item.Isdefaulttax ?? false,
                TaxPercentage = item.Taxpercentage ?? 0,
                ShortCode = item.Shortcode,
                Description = item.Description,
                ImagePath = item.Image,
                Isfavourite = item.Isfavourite ?? true,
                Modifiergroup = item.Mappingmenuitemwithmodifiers.Select(mg => new ModifiergroupDto
                {
                    ModifierGroupId = mg.Modifiergroupid,
                    MinSelection = mg.Minselectionrequired ?? 0,
                    MaxSelection = mg.Maxselectionallowed ?? 1,
                    Name = mg.Modifiergroup.Name,
                }).Distinct().ToList()
            };
            return items;
        }
        return new MenuItems();
    }

    public ModifierGroupDto GetModifierGroupById(int id)
    {
        var modifierGroup = _db.Modifiergroups.FirstOrDefault(u => u.Id == id);
        ModifierGroupDto modifiergroup = new()
        {
            id = modifierGroup.Id,
            ModifiergroupName = modifierGroup.Name
        };
        return modifiergroup;
    }


    public FilterPaginationDto<CategoryModifierDto> pagination(int categoryId,int pageNumber, int pageSize, string searchString)
    {
         string defaultImagePath = "/images/dining-menu.png";

        var query = _db.Menuitems
            .Include(i => i.Mappingmenuitemwithmodifiers).ThenInclude(m => m.Modifiergroup)
            .Include(i => i.Unit)
            .Where(i => i.Categoryid ==  categoryId && i.Isdeleted==false);

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(i => i.Name.Contains(searchString));
        }

        int totalItems = query.Count();

        var pagedItems = query
            .OrderBy(i => i.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(i => new MenuItems
            {
                CategoryId = i.Categoryid,
                itemId = i.Id,
                Type = i.Type ?? true,
                UnitId = i.Unitid ?? 0,
                Name = i.Name,
                Price = i.Rate,
                Quantity = i.Quantity ?? 0,
                IsAvailable = i.Isavailable ?? true,
                ImagePath = string.IsNullOrEmpty(i.Image) ? defaultImagePath : i.Image,
                UnitName = i.Unit.Name,
                IsDefaultTax = i.Isdefaulttax ?? true,
                Isfavourite = i.Isfavourite ?? false,
                ShortCode = i.Shortcode,
                Description = i.Description,
                Modifiergroup = i.Mappingmenuitemwithmodifiers.Select(mi => new ModifiergroupDto
                {
                    Name = mi.Modifiergroup.Name,
                    MinSelection = mi.Minselectionrequired ?? 0,
                    MaxSelection = mi.Maxselectionallowed ?? 0,
                    ModifierGroupId = mi.Modifiergroupid
                }).ToList()
            }).ToList();

     
        var categoryModifierDto = new CategoryModifierDto
        {
            menuItems = pagedItems
        };

        FilterPaginationDto<CategoryModifierDto> filterPaginationDto = new()
        {
            Item = categoryModifierDto,
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
            SearchString = searchString
        };
        return filterPaginationDto;
    }
    public FilterPaginationDto<ModifiersDto> GetAllModifiers(int pageNumber = 1, int pageSize = 5, string searchString = "")
{
    // Build the query
    var query = _db.Modifiers.Include(u => u.Unit).AsQueryable();

    // Apply filter if searchString is provided (you can filter by modifier name or description)
    if (!string.IsNullOrEmpty(searchString))
    {
        query = query.Where(u => u.Name.Contains(searchString) || u.Description.Contains(searchString));
    }

    // Get the total count of items before pagination
    var totalItems = query.Count();

    // Apply pagination
    var modifiersDtos = query
        .Skip((pageNumber - 1) * pageSize)  // Skip items for previous pages
        .Take(pageSize)  // Take the number of items per page
        .Select(u => new ModifiersDto
        {
            unit = u.Unit.Name,
            id = u.Id,
            modifierGroupID = u.Modifiergroupid,
            name = u.Name,
            rate = u.Rate,
            quantity = u.Quantity,
            description = u.Description
        })
        .ToList();

    // Create and return the pagination data
    var paginationResult = new FilterPaginationDto<ModifiersDto>
    {
        Items = modifiersDtos,
        TotalItems = totalItems,
        PageNumber = pageNumber,
        PageSize = pageSize,
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
    };

    return paginationResult;
}



    
}