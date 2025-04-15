using System.Drawing;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using pizzashop_Repository.Interface;
using pizzashop_Repository.ViewModel;
using pizzashop_Services.interfaceService;
using SelectPdf;

namespace pizzashop_Services.ImplementationService;

public class OrderExport_Service : IOrderExport_Service
{
    private readonly IOrderExport_Repository orderExportRepository;
    public OrderExport_Service(IOrderExport_Repository _orderExportRepository)
    {
        orderExportRepository = _orderExportRepository;
    }

    public List<OrdersExportDto> GetFilteredOrders(string searchString, string orderStatus, string dateRange)
    {
        return orderExportRepository.GetOrdersDetail(searchString, orderStatus, dateRange);
    }


    public FilterPaginationDto<OrdersExportDto> GetFilterOrders(int pageNumber, int pageSize, string searchString, string sortOrder, string orderStatus, string dateRange, string fromDate, string toDate)
    {
        List<OrdersExportDto> orders = orderExportRepository.GetFilterOrders(searchString, orderStatus, dateRange, sortOrder, pageNumber, pageSize, fromDate, toDate);
        int count = orderExportRepository.countOrders(searchString, orderStatus, dateRange, fromDate, toDate);
        FilterPaginationDto<OrdersExportDto> orderExport = new FilterPaginationDto<OrdersExportDto>();
        orderExport.SetPaginationData(orders, count, pageNumber, pageSize, searchString);
        return orderExport;
    }

    public List<OrdersExportDto> GetOrders()
    {
        return orderExportRepository.GetOrdersDetail();
    }

    public FileResult ExportOrdersToExcel(string searchString, string orderStatus, string dateRange)
    {
        var orders = orderExportRepository.GetOrdersDetail(searchString, orderStatus, dateRange);
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (ExcelPackage package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Orders");
            worksheet.Cells["B2"].Value = "Status:";
            worksheet.Cells["B3"].Value = "Date:";
            worksheet.Cells["E2"].Value = "Search Text:";
            worksheet.Cells["E3"].Value = "No. Of Records:";
            worksheet.Cells["C2"].Value = orderStatus;
            worksheet.Cells["C3"].Value = dateRange;
            worksheet.Cells["F2"].Value = searchString;
            worksheet.Cells["F3"].Value = orders.Count;

            // 🔹 **Style for Metadata Labels (B2, B3, E2, E3)**
            using (var range = worksheet.Cells["B2:B3"])
            {
                range.Style.Font.Bold = true;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.Font.Color.SetColor(Color.White);
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#0066A7"));
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            }

            using (var range = worksheet.Cells["E2:E3"])
            {
                range.Style.Font.Bold = true;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.Font.Color.SetColor(Color.White);
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#0066A7"));
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            }

            // 🔹 **Style for Metadata Values (C2, C3, F2, F3)**
            using (var range = worksheet.Cells["C2:C3"])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.Font.Color.SetColor(Color.Black);
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.White);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            }

            using (var range = worksheet.Cells["F2:F3"])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.Font.Color.SetColor(Color.Black);
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.White);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            }
                
            string logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "logos", "pizzashop_logo.png");
            if (File.Exists(logoPath))
            {
                var logo = worksheet.Drawings.AddPicture("Logo", new FileInfo(logoPath));
                logo.SetPosition(1, 0, 7, 0);
                logo.SetSize(60, 50);
            }


            else
            {
                throw new FileNotFoundException("Logo file not found: " + logoPath);
            }
            string[] headers = { "Id", "Date", "Customer", "Status", "Payment Mode", "Rating", "Total Amount" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[5, i + 2].Value = headers[i];
            }

            using (var headerRange = worksheet.Cells[5, 2, 5, 8])
            {
                headerRange.Style.Font.Bold = true;
                headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                headerRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                headerRange.Style.Font.Color.SetColor(Color.White);
                headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#0066A7"));
                headerRange.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            }

            // 🔹 **Data Insertion**
            int row = 6;
            foreach (var order in orders)
            {
                worksheet.Cells[row, 2].Value = order.orderID;
                worksheet.Cells[row, 3].Value = order.Date.ToString("dd-MM-yyyy HH:mm:ss");
                worksheet.Cells[row, 4].Value = order.CustomerName;
                worksheet.Cells[row, 5].Value = order.Status;
                worksheet.Cells[row, 6].Value = order.PaymentMode;
                worksheet.Cells[row, 7].Value = order.Rating;
                worksheet.Cells[row, 8].Value = order.TotalAmount;
                row++;
            }

            worksheet.Cells.AutoFitColumns();
            var stream = new MemoryStream(package.GetAsByteArray());
            return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "Orders.xlsx"
            };
        }
    }

    public OrderDetailsDto GetInvoice(int orderId)
    {
        return orderExportRepository.GetInvoice(orderId);
    }

}