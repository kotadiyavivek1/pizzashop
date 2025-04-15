using Microsoft.AspNetCore.Mvc;
using pizzashop_Repository.ViewModel;

namespace pizzashop_Repository.ImplementationService;
public interface ICustomer_Service
{
    public FilterPaginationDto<customerDetailsDto> filterPaginationDto(int pageNumber, int pageSize, string searchString, string sortOrder, string dateRange, string fromDate , string toDate ); 
    public CustomerHistoryDto GetCustomerHistory(int customerId);
    public FileResult ExportCustomerToExcel(string searchString, string dateRange,string fromDate, string toDate,string Email);

}