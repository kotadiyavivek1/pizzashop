using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using pizzashop_Repository.Interface;
using pizzashop_Repository.Models;
using pizzashop_Repository.ViewModel;

namespace pizzashop_Repository.Implementation;

public class Customer_Repository : ICustomer_Repository
{
    private readonly PizzashopContext context;
    public Customer_Repository(PizzashopContext _context)
    {
        context = _context;
    }

    public int countCustomers(string searchString, string dateRange, string fromDate, string toDate)
    {

        IQueryable<Order> ordersQuery = context.Orders.Include(o => o.Customer);

        if (!string.IsNullOrEmpty(searchString))
        {
            string searchLower = searchString.ToLower();
            ordersQuery = ordersQuery.Where(o =>
                o.Id.ToString().Contains(searchLower) ||
                (o.Customer != null && o.Customer.Name.ToLower().Contains(searchLower))
            );
        }

        DateTime currentDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        if (!string.IsNullOrEmpty(dateRange))
        {
            switch (dateRange.ToLower())
            {
                case "last 7 days":
                    ordersQuery = ordersQuery.Where(o => o.Createdat >= currentDate.AddDays(-7));
                    break;
                case "last 30 days":
                    ordersQuery = ordersQuery.Where(o => o.Createdat >= currentDate.AddDays(-30));
                    break;
                case "current month":
                    ordersQuery = ordersQuery.Where(o => o.Createdat.Year == currentDate.Year && o.Createdat.Month == currentDate.Month);
                    break;
            }
        }

        if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate) &&
            DateTime.TryParse(fromDate, out DateTime parsedFromDate) &&
            DateTime.TryParse(toDate, out DateTime parsedToDate))
        {
            ordersQuery = ordersQuery.Where(o => o.Createdat >= parsedFromDate && o.Createdat <= parsedToDate);
        }

        IQueryable<customerDetailsDto>? groupedQuery = ordersQuery
           .GroupBy(o => new
           {
               CreatedDate = o.Createdat.Date,
               CustomerName = o.Customer.Name,
               EmailId = o.Customer.Email,
               PhoneNumber = o.Customer.Phone
           })
           .Select(g => new customerDetailsDto
           {
               Date = g.Key.CreatedDate,
               Name = g.Key.CustomerName,
               Email = g.Key.EmailId,
               PhoneNumber = g.Key.PhoneNumber.ToString(),
               CountOrders = g.Count()
           });
        return groupedQuery.Count();

    }
    public string GetUserDetails(string Email)
    {
        return context.Users
            .Where(u => u.Email == Email)
            .Select(u => u.Role.Name)
            .FirstOrDefault() ?? "Unknown";
    }
    public List<customerDetailsDto> GetCustomerDetails(string searchString, string dateRange, string fromDate, string toDate)
    {
        IQueryable<Order> ordersQuery = context.Orders.Include(o => o.Customer);
        if (!string.IsNullOrEmpty(searchString))
        {
            string searchLower = searchString.ToLower();
            ordersQuery = ordersQuery.Where(o =>
                o.Id.ToString().Contains(searchLower) ||
                (o.Customer != null && o.Customer.Name.ToLower().Contains(searchLower))
            );
        }
        DateTime currentDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        if (!string.IsNullOrEmpty(dateRange))
        {
            switch (dateRange.ToLower())
            {
                case "last 7 days":
                    ordersQuery = ordersQuery.Where(o => o.Createdat >= currentDate.AddDays(-7));
                    break;
                case "last 30 days":
                    ordersQuery = ordersQuery.Where(o => o.Createdat >= currentDate.AddDays(-30));
                    break;
                case "current month":
                    ordersQuery = ordersQuery.Where(o => o.Createdat.Year == currentDate.Year && o.Createdat.Month == currentDate.Month);
                    break;
            }
        }

        if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate) &&
       DateTime.TryParse(fromDate, out DateTime parsedFromDate) &&
       DateTime.TryParse(toDate, out DateTime parsedToDate))
        {
            parsedFromDate = DateTime.SpecifyKind(parsedFromDate, DateTimeKind.Unspecified);
            parsedToDate = DateTime.SpecifyKind(parsedToDate, DateTimeKind.Unspecified).AddDays(1).AddTicks(-1); // End of day

            ordersQuery = ordersQuery.Where(o => o.Createdat >= parsedFromDate && o.Createdat <= parsedToDate);
        }

        IQueryable<customerDetailsDto> groupedQuery = ordersQuery
       .GroupBy(o => new
       {
           customerId = o.Customer.Id,
           CreatedDate = o.Createdat.Date,
           CustomerName = o.Customer != null ? o.Customer.Name : "Unknown",
           EmailId = o.Customer != null ? o.Customer.Email : "Unknown",
           PhoneNumber = o.Customer != null ? o.Customer.Phone.ToString() : "Unknown"
       })
       .Select(g => new customerDetailsDto
       {
           customerId = g.Key.customerId,
           Date = g.Key.CreatedDate,
           Name = g.Key.CustomerName,
           Email = g.Key.EmailId,
           PhoneNumber = g.Key.PhoneNumber,
           CountOrders = g.Count()
       });
        return groupedQuery.ToList();
    }

    public List<customerDetailsDto> FilterCustomer(
    string searchString,
    string dateRange,
    string sortOrder,
    int pageNumber,
    int pageSize,
    string fromDate,
    string toDate)
    {
        IQueryable<Order> ordersQuery = context.Orders.Include(o => o.Customer);
        if (!string.IsNullOrEmpty(searchString))
        {
            string searchLower = searchString.ToLower();
            ordersQuery = ordersQuery.Where(o =>
                o.Id.ToString().Contains(searchLower) ||
                (o.Customer != null && o.Customer.Name.ToLower().Contains(searchLower))
            );
        }
        DateTime currentDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        if (!string.IsNullOrEmpty(dateRange))
        {
            switch (dateRange.ToLower())
            {
                case "last 7 days":
                    ordersQuery = ordersQuery.Where(o => o.Createdat >= currentDate.AddDays(-7));
                    break;
                case "last 30 days":
                    ordersQuery = ordersQuery.Where(o => o.Createdat >= currentDate.AddDays(-30));
                    break;
                case "current month":
                    ordersQuery = ordersQuery.Where(o => o.Createdat.Year == currentDate.Year && o.Createdat.Month == currentDate.Month);
                    break;
            }
        }


        if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate) &&
            DateTime.TryParse(fromDate, out DateTime parsedFromDate) &&
            DateTime.TryParse(toDate, out DateTime parsedToDate))
        {
            parsedFromDate = DateTime.SpecifyKind(parsedFromDate, DateTimeKind.Unspecified);
            parsedToDate = DateTime.SpecifyKind(parsedToDate, DateTimeKind.Unspecified).AddDays(1).AddTicks(-1); // End of day

            ordersQuery = ordersQuery.Where(o => o.Createdat >= parsedFromDate && o.Createdat <= parsedToDate);
        }

        IQueryable<customerDetailsDto> groupedQuery = ordersQuery
            .GroupBy(o => new
            {
                customerId = o.Customer.Id,
                CreatedDate = o.Createdat.Date,
                CustomerName = o.Customer != null ? o.Customer.Name : "Unknown",
                EmailId = o.Customer != null ? o.Customer.Email : "Unknown",
                PhoneNumber = o.Customer != null ? o.Customer.Phone.ToString() : "Unknown"
            })
            .Select(g => new customerDetailsDto
            {
                customerId = g.Key.customerId,
                Date = g.Key.CreatedDate,
                Name = g.Key.CustomerName,
                Email = g.Key.EmailId,
                PhoneNumber = g.Key.PhoneNumber,
                CountOrders = g.Count()
            });

        groupedQuery = sortOrder?.ToLower() switch
        {
            "date_asc" => groupedQuery.OrderBy(x => x.Date),
            "date_desc" => groupedQuery.OrderByDescending(x => x.Date),
            "name_asc" => groupedQuery.OrderBy(x => x.Name),
            "name_desc" => groupedQuery.OrderByDescending(x => x.Name),
            "count_asc" => groupedQuery.OrderBy(x => x.CountOrders),
            "count_desc" => groupedQuery.OrderByDescending(x => x.CountOrders),
            _ => groupedQuery.OrderBy(x => x.Date) // Default sorting
        };
        // Apply pagination
        return groupedQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public CustomerHistoryDto GetCustomerHistory(int customerId)
    {
        var customerHistory = context.Customers
            .Where(c => c.Id == customerId)
            .Select(c => new CustomerHistoryDto
            {
                Name = c.Name,
                MobileNumber = c.Phone.ToString(),
                AverageBill = c.Orders.Any() ? c.Orders.Average(o => o.Totalamount) : 0,
                ComingSince = c.Orders.Any() ? c.Orders.Min(o => o.Createdat).ToString("yyyy-MM-dd") : "N/A",
                MaxOrder = c.Orders.Any() ? c.Orders.Max(o => o.Totalamount) : 0,
                Visits = c.Orders.Select(o => o.Createdat.Date)
                                 .Distinct()
                                 .Count(),
                orderDetails = c.Orders.Select(o => new OrderDetailDto
                {
                    OrderId = o.Id,
                    OrderType = "DineIn",
                    PaymentMethod = o.Payment != null ? o.Payment.Paymentmethod : "N/A",
                    Items = o.Ordereditems.Count(),
                    orderDate = o.Createdat.ToString(),
                    Amount = o.Totalamount ?? 0
                }).ToList()
            })
            .FirstOrDefault();

        return customerHistory ?? new CustomerHistoryDto();
    }
}
