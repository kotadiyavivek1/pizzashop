using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;
using pizzashop_Repository.Implementation;
using pizzashop_Repository.ViewModel;

namespace pizzashop_Repository.Interface;

public interface IOrderExport_Repository
{ 
    public List<OrdersExportDto> GetOrdersDetail();
    public List<OrdersExportDto> GetFilterOrders(string searchString, string orderStatus, string dateRange, string sortOrder, int pageNumber, int pageSize,string fromDate,string toDate);
    public int countOrders(string searchString, string orderStatus, string dateRange, string fromDate, string toDate);
    public List<OrdersExportDto> GetOrdersDetail(string searchString, string orderStatus,string dateRange);
    public OrderDetailsDto GetInvoice(int orderId);
}
