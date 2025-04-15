using Microsoft.EntityFrameworkCore.Metadata.Internal;
using pizzashop_Repository.Implementation;
using pizzashop_Repository.ViewModel;

namespace pizzashop_Repository.Interface;

public interface ITableSection_Repository
{
    public List<SectionDto> GetSections();
    public List<TableDto> GetTablesBySection(int sectionId);

    public List<TableDto> FilterTable(int sectionId,int PageNumber,int PageSize,string searchString);
    public  int CountTable(int sectionId,string searchString);
    public bool AddSection(TableSectionDto tableSectionDto,string email);
    public bool EditSection(int Id,SectionDto sectionDto,string email); 
    public SectionDto GetSectionById(int sectionId);
    public bool DeleteSection(int sectionId);
    public bool AddTable(TableSectionDto tableSectionDto,string email);
    public TableDto GetTableById(int tableId);
    public bool EditTable(int Id,TableDto tableDto,string email);
    public bool DeleteTable(int tableId);
    public bool DeleteTables(List<int> tableIds);
}
