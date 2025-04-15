using pizzashop_Repository.ViewModel;

namespace pizzashop_Repository.Interface;

public interface ICustomer_Repository
{
    public List<customerDetailsDto> FilterCustomer(string searchString,string dateRange,string sortOrder,int pageNumber,int pageSize,string fromDate,string toDate);
    public int countCustomers(string searchString,string dateRange, string fromDate, string toDate);
    public CustomerHistoryDto GetCustomerHistory(int customerId);
    public List<customerDetailsDto> GetCustomerDetails(string searchString, string dateRange, string fromDate, string toDate);
    public string GetUserDetails(string Email);
}
