using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pizzashop_Repository.ViewModel;
using pizzashop_Services.interfaceService;
using SelectPdf;

namespace pizzashop.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderExport_Service ordersExportDto;
        private readonly IViewRender_Service viewRenderService;

        public OrdersController(IOrderExport_Service _orderExportDto, IViewRender_Service _viewRenderService)
        {
            ordersExportDto = _orderExportDto;
            viewRenderService = _viewRenderService;
        }

        

        public IActionResult OrderExport()
        {
            return View();
        }

        public IActionResult FilterOrder(int pageNumber, int pageSize, string searchString, string sortOrder, string dateRange, string orderStatus, string fromDate, string toDate)
        {
            FilterPaginationDto<OrdersExportDto> orders = ordersExportDto.GetFilterOrders(pageNumber, pageSize, searchString, sortOrder, orderStatus, dateRange, fromDate, toDate);
            return PartialView("_OrderTable", orders);
        }
        [HttpGet]
        public IActionResult ExportToExcel(string searchString = "", string orderStatus = "All", string dateRange = "All time")
        {
            return ordersExportDto.ExportOrdersToExcel(searchString, orderStatus, dateRange);
        }
        public IActionResult OrderInvoice(int orderid)
        {
            OrderDetailsDto orderDetails = ordersExportDto.GetInvoice(orderid);
            return View(orderDetails);
        }

        public async Task<IActionResult> GenerateInvoicePdf(int orderId)
        {
            var orderDetails = ordersExportDto.GetInvoice(orderId);
            if (orderDetails == null)
            {
                return NotFound();
            }
            string htmlContent =await viewRenderService.RenderToStringAsync("Orders/InvoicePdf", orderDetails);
            HtmlToPdf converter = new HtmlToPdf();
            PdfDocument doc = converter.ConvertHtmlString(htmlContent);

            using (MemoryStream pdfStream = new MemoryStream())
            {
                doc.Save(pdfStream);
                doc.Close();
                pdfStream.Position = 0;
                return File(pdfStream.ToArray(), "application/pdf", $"Invoice_{orderId}.pdf");
            }
        }
    }

}
