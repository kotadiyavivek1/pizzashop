using Microsoft.AspNetCore.Mvc;
using pizzashop_Repository.ViewModel;

namespace pizzashop_Services.interfaceService;

public interface IOrderExport_Service
{
    public List<OrdersExportDto> GetOrders();
    public FilterPaginationDto<OrdersExportDto> GetFilterOrders(int pageNumber,int pageSize,string searchString,string sortOrder,string orderStatus,string dateRange,string fromDate,string toDate);    public List<OrdersExportDto> GetFilteredOrders(string searchString,string orderStatus,string dateRange);
   FileResult ExportOrdersToExcel(string searchString, string orderStatus, string dateRange);
   public OrderDetailsDto GetInvoice(int orderId);
}
