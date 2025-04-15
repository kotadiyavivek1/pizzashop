using Microsoft.AspNetCore.Mvc;
using pizzashop_Repository.ImplementationService;
using pizzashop_Repository.ViewModel;

namespace pizzashop.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomer_Service customerService;
        public CustomersController(ICustomer_Service _customerService)
        {
            customerService = _customerService;
        }
        public IActionResult customerExport()
        {
            return View();
        }
        [HttpGet]
        [Route("/Customers/FilterCustomers")]
        public IActionResult FilterCustomers(int pageNumber = 1, int pageSize = 5, string searchString = "", string sortOrder = "", string dateRange = "All time", string fromDate = "", string toDate = "")
        {
            FilterPaginationDto<customerDetailsDto> modal = customerService.filterPaginationDto(pageNumber,pageSize,searchString,sortOrder,dateRange,fromDate,toDate);
            return PartialView("_customerTable",modal);
        }

        [HttpGet]
        [Route("/Customers/GetCustomerHistory")]
        public IActionResult GetCustomerHistory(int customerId)
        {
            return Json(customerService.GetCustomerHistory(customerId));
        }
        [HttpGet]
        public IActionResult ExportToExcel(string searchString, string dateRange,string fromDate, string toDate)
        {
            string Email = Request.Cookies["Email"] ?? "";
            return customerService.ExportCustomerToExcel(searchString, dateRange, fromDate,toDate,Email);
        }
    
    }
}