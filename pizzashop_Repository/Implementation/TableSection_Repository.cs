using System.Security.AccessControl;
using Microsoft.EntityFrameworkCore.Metadata;
using pizzashop_Repository.Interface;
using pizzashop_Repository.Models;
using pizzashop_Repository.ViewModel;

namespace pizzashop_Repository.Implementation;


public class TableSection_Repository : ITableSection_Repository
{
    private readonly PizzashopContext _db;
    public TableSection_Repository(PizzashopContext db)
    {
        _db = db;
    }


    public bool AddSection(TableSectionDto tableSectionDto, string email)
    {
        
            User user = _db.Users.FirstOrDefault(u => u.Email == email) ?? new User();
            if (user != null && tableSectionDto.SectionDto!=null && tableSectionDto!=null )
            {
                
                Section section = new Section()
                {
                    Name = tableSectionDto.SectionDto.SectionName ?? "",
                    Description = tableSectionDto.SectionDto.Description ?? "",
                    Createdby=user.Id,
                    Createdat=DateTime.Now,
                    
                };
                _db.Sections.Add(section);
                _db.SaveChanges();
                return true;
            }
            return false;
    }

    public bool AddTable(TableSectionDto tableSectionDto, string email)
    {
        if(tableSectionDto.TableDto != null)
        {
        User user = _db.Users.FirstOrDefault(u => u.Email == email) ?? new User();
        Table table = _db.Tables.FirstOrDefault(t => t.Name == tableSectionDto.TableDto.TableName) ?? new Table();
        if (user != null && tableSectionDto.TableDto != null  && table!=null)
        {
            Table newTable = new Table()
            {
                Name = tableSectionDto.TableDto.TableName ?? "",
                Sectionid = tableSectionDto.TableDto.SectionId,
                Isavailable = tableSectionDto.TableDto.IaAvailable,
                Capacity = tableSectionDto.TableDto.Capacity,
                Createdby = user.Id,
                Createdat = DateTime.Now,
            };
            _db.Tables.Add(newTable);
            _db.SaveChanges();
            return true;
        }
        return false;
        }
        return false;
    }

    public int CountTable(int sectionId, string searchString)
    {
        var query = _db.Tables.Where(t => t.Sectionid == sectionId && t.Isdeleted==false).AsQueryable();
        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(t => t.Name.Contains(searchString));
        }
        return query.Count();
    }

    public bool DeleteSection(int sectionId)
    {
        Section section = _db.Sections.FirstOrDefault(s => s.Id == sectionId) ?? new Section();
        if (section != null)
        {
            section.Isdeleted = true;
            _db.SaveChanges();
            return true;
        }
        return false;
    }

    public bool DeleteTable(int tableId)
    {
        Table table = _db.Tables.FirstOrDefault(t => t.Id == tableId) ?? new Table();
        if (table != null)
        {
            table.Isdeleted = true;
            _db.SaveChanges();
            return true;
        }
        return false;
    }

    public bool DeleteTables(List<int> tableIds)
    {
        foreach (var tableId in tableIds)
        {
            Table table = _db.Tables.FirstOrDefault(t => t.Id == tableId) ?? new Table();
            if (table != null)
            {
                table.Isdeleted = true;
                _db.SaveChanges();
            }
        }
        return true;
    }


    public bool EditSection(int Id, SectionDto sectionDto, string email)
    {
        
        if(sectionDto!=null)
        {
            User user = _db.Users.FirstOrDefault(u => u.Email == email) ?? new User();
            Section section = _db.Sections.FirstOrDefault(s => s.Id == Id) ?? new Section();
            if (section != null)
            {
                section.Name = sectionDto.SectionName ?? "";
                section.Description = sectionDto.Description ?? "";
                section.Modifiedby = user.Id;
                section.Modifiedat= DateTime.Now;
                _db.SaveChanges();
                return true;
            }
            return false;
        }
        return false;
    }

    public bool EditTable(int Id, TableDto tableDto, string email)
    {
        if(tableDto!=null)
        {
            User user = _db.Users.FirstOrDefault(u => u.Email == email) ?? new User();
            Table table = _db.Tables.FirstOrDefault(t => t.Id == Id) ?? new Table();
            if (table != null)
            {
                table.Name = tableDto.TableName ?? "";
                table.Sectionid = tableDto.SectionId;
                table.Isavailable = tableDto.IaAvailable;
                table.Capacity = tableDto.Capacity;
                table.Modifiedby = user.Id;
                table.Modifiedat = DateTime.Now;
                _db.Update(table);
                _db.SaveChanges();
                return true;
            }
            return false;
        }
        return false;
    }

    public List<TableDto> FilterTable(int sectionId, int PageNumber, int PageSize, string searchString)
    {
        IQueryable<Table> tables = _db.Tables.Where(s => s.Sectionid == sectionId && s.Isdeleted == false).OrderBy(t => t.Id).AsQueryable();
        if (string.IsNullOrEmpty(searchString))
        {
            return tables.Skip((PageNumber - 1) * PageSize).Take(PageSize).Where(t => t.Isdeleted == false).Select(t => new TableDto
            {
                Id = t.Id,
                TableName = t.Name,
                SectionId = t.Sectionid,
                IaAvailable = t.Isavailable ?? true,
                Capacity = t.Capacity ?? 0,
            }).ToList();
        }
        tables = (IQueryable<Table>)tables.Where(t => t.Name.Contains(searchString));
        return tables.Skip((PageNumber - 1) * PageSize).Take(PageSize).Where(t => t.Isdeleted == false).Select(t => new TableDto
        {
            Id = t.Id,
            TableName = t.Name,
            SectionId = t.Sectionid,
            IaAvailable = t.Isavailable ?? true,
            Capacity = t.Capacity ?? 0,
        }).ToList();
    }

    public SectionDto GetSectionById(int sectionId)
    {
        Section section = _db.Sections.Where(s=>s.Isdeleted==false).FirstOrDefault(s => s.Id == sectionId) ?? new Section();
        return new SectionDto()
        {
            Id = section.Id,
            SectionName = section.Name,
            Description = section.Description,
        };
    }

    public List<SectionDto> GetSections()
    {
        return _db.Sections.Where(s => s.Isdeleted == false).Select(s => new SectionDto()
        {
            Id = s.Id,
            SectionName = s.Name,
            Description = s.Description,
        }).OrderBy(s=>s.Id).ToList();
    }

    public TableDto GetTableById(int tableId)
    {
        Table table = _db.Tables.FirstOrDefault(t => t.Id == tableId) ?? new Table();
        return new TableDto()
        {
            Id = table.Id,
            TableName = table.Name,
            SectionId = table.Sectionid,
            IaAvailable = table.Isavailable ?? true,
            Capacity = table.Capacity ?? 0,
        };
    }


    public List<TableDto> GetTablesBySection(int sectionId)
    {
        return _db.Tables.Where(s => s.Section.Id == sectionId).Select(s => new TableDto
        {
            Id = s.Id,
            TableName = s.Name,
            IaAvailable = s.Isavailable ?? true,
            Capacity = s.Capacity ?? 0,
            SectionId = s.Sectionid,
        }).ToList();
    }

   

}
