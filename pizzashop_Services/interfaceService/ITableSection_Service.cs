using Microsoft.EntityFrameworkCore.Metadata.Internal;
using pizzashop_Repository.ViewModel;

namespace pizzashop_Services.interfaceService;

public interface ITableSection_Service
{
    public TableSectionDto getAllItem();
    public List<TableDto> getTablesBySection(int sectionId);
    public FilterPaginationDto<TableDto> FilterTable(int sectionId,int PageNumber,int PageSize,string searchString);
    public bool AddSection(TableSectionDto tableSectionDto,string email);
    public bool EditSection(int Id,SectionDto sectionDto,string email);
    public SectionDto GetSectionById(int sectionId);
    public bool DeleteSection(int sectionId);
    public bool AddTable(TableSectionDto tableSectionDto,string email);
    public TableSectionDto GetTableById(int tableId);
    public bool EditTable(int Id,TableDto tableDto,string email);
    public bool DeleteTable(int tableId);    
    public bool DeleteTables(List<int> tableIds);
}
