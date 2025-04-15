using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using pizzashop_Repository.ImplementationService;
using pizzashop_Repository.Interface;
using pizzashop_Repository.ViewModel;

namespace pizzashop_Services.ImplementationService;
public class Customer_Service : ICustomer_Service
{
    private readonly ICustomer_Repository customerRepository;
    public Customer_Service(ICustomer_Repository _customerRepository)
    {
        customerRepository = _customerRepository;
    }
    public FilterPaginationDto<customerDetailsDto> filterPaginationDto(int pageNumber, int pageSize, string searchString, string sortOrder, string dateRange, string fromDate, string toDate)
    {
        List<customerDetailsDto> customers = customerRepository.FilterCustomer(searchString,dateRange,sortOrder,pageNumber,pageSize,fromDate,toDate);
        int countCustomers = customerRepository.countCustomers(searchString,dateRange,fromDate,toDate);
        FilterPaginationDto<customerDetailsDto> dto = new();
        dto.SetPaginationData(customers,countCustomers,pageNumber,pageSize,searchString);
        return dto;
    }

    public CustomerHistoryDto GetCustomerHistory(int customerId)
    {
        return customerRepository.GetCustomerHistory(customerId);
    }

    public FileResult ExportCustomerToExcel(string searchString, string dateRange,string fromDate, string toDate,string Email)   
    {
        List<customerDetailsDto> orders = customerRepository.GetCustomerDetails(searchString, dateRange, fromDate, toDate);
        string user = customerRepository.GetUserDetails(Email);
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (ExcelPackage package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Orders");
            worksheet.Cells["B2"].Value = "Account";
            worksheet.Cells["B3"].Value = "Date:";
            worksheet.Cells["E2"].Value = "Search Text:";
            worksheet.Cells["E3"].Value = "No. Of Records:";
            worksheet.Cells["C2"].Value = user;
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
            string[] headers = { "Name", "Email", "PhoneNumber", "Date", "Total Orders" };
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

            int row = 6;
            foreach (var order in orders)
            {
                worksheet.Cells[row, 2].Value = order.Name;
                worksheet.Cells[row, 3].Value = order.Email;
                worksheet.Cells[row, 4].Value = order.PhoneNumber;
                worksheet.Cells[row, 5].Value = order.Date;
                worksheet.Cells[row, 6].Value = order.CountOrders;
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

}