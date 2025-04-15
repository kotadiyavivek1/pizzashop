using pizzashop_Repository.ViewModel;
using pizzashop_Repository.Interface;
using pizzashop_Services.interfaceService;
using Microsoft.AspNetCore.Mvc;
namespace pizzashop_Repository.Implementation;
public class TableSection_Service : ITableSection_Service
{
    private readonly ITableSection_Repository tableSectionRepository;
    public TableSection_Service(ITableSection_Repository _tableSection_Repository)
    {
        tableSectionRepository = _tableSection_Repository;
    }

    public bool AddSection(TableSectionDto tableSectionDto,string email)
    {
        if(tableSectionRepository.AddSection(tableSectionDto,email))
        {
            return true;
        }
        return false;
    }

    public bool EditSection(int Id, SectionDto sectionDto,string email)
    {
        if(tableSectionRepository.EditSection(Id,sectionDto,email))
        {
            return true;
        }
        return false;
    }

    public FilterPaginationDto<TableDto> FilterTable(int sectionId, int PageNumber=1, int PageSize=3, string searchString="")
    {
        List<TableDto> tableDtos = tableSectionRepository.FilterTable(sectionId,PageNumber,PageSize,searchString);
        int totalTables = tableSectionRepository.CountTable(sectionId,searchString);
        FilterPaginationDto<TableDto> filterPaginationDto = new FilterPaginationDto<TableDto>();
        filterPaginationDto.SetPaginationData(tableDtos,totalTables,PageNumber,PageSize,searchString);
        return filterPaginationDto;
    }


    public TableSectionDto getAllItem() 
    {
        List<SectionDto> sections =  tableSectionRepository.GetSections();
        TableSectionDto dto = new TableSectionDto();
        dto.Sections = sections;
        return dto;
    }
    public List<TableDto> getTablesBySection(int sectionId)
    {
        return tableSectionRepository.GetTablesBySection(sectionId);
    }
    [HttpGet]
    public SectionDto GetSectionById(int sectionId){
        return tableSectionRepository.GetSectionById(sectionId);
    }

    public bool DeleteSection(int sectionId)
    {
        if(tableSectionRepository.DeleteSection(sectionId))
        {
            return true;
        }
        return false;
    }

    public bool AddTable(TableSectionDto tableSectionDto,string email)
    {
        if(tableSectionRepository.AddTable(tableSectionDto,email))
        {
            return true;
        }
        return false;
    }

    public TableSectionDto GetTableById(int tableId)
    {
        TableDto table =  tableSectionRepository.GetTableById(tableId);
        List<SectionDto> sections =  tableSectionRepository.GetSections();
        TableSectionDto dto = new TableSectionDto();
        dto.TableDto = table;
        dto.Sections= sections;
        return dto;
    }

    public bool EditTable(int Id, TableDto tableDto, string email)
    {
        if(tableSectionRepository.EditTable(Id,tableDto,email))
        {
            return true;
        }
        return false;
    }

    public bool DeleteTable(int tableId)
    {
        if(tableSectionRepository.DeleteTable(tableId))
        {
            return true;
        }
        return false;
    }

    public bool DeleteTables(List<int> tableIds)
    {
        if(tableSectionRepository.DeleteTables(tableIds))
        {
            return true;
        }
        return false;
    }

}
