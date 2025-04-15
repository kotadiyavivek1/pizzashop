namespace pizzashop_Repository.ViewModel;
public class TableSectionDto
{
    public TableDto? TableDto{ get; set; }
    public SectionDto? SectionDto{ get; set; }   
    public List<TableDto>? Tables{ get; set; }
    public FilterPaginationDto<List<TableDto>>? FilterPaginationDto{ get; set; }
    public List<SectionDto>? Sections{ get; set; }
}