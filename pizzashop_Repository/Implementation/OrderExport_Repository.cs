using Microsoft.EntityFrameworkCore;
using pizzashop_Repository.Interface;
using pizzashop_Repository.Models;
using pizzashop_Repository.ViewModel;

namespace pizzashop_Repository.Implementation;

public class OrderExport_Repository : IOrderExport_Repository
{
    private readonly PizzashopContext context;
    public OrderExport_Repository(PizzashopContext _context)
    {
        context = _context;
    }


    public List<OrdersExportDto> GetFilterOrders(string searchString, string orderStatus, string dateRange, string sortOrder, int pageNumber, int pageSize, string fromDate, string toDate)
    {
        IQueryable<Order> orders = context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Feedbacks)
            .Include(o => o.Payment)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            string searchLower = searchString.ToLower();
            orders = orders.Where(o =>
                o.Id.ToString().Contains(searchLower) ||
                (o.Customer != null && o.Customer.Name.ToLower().Contains(searchLower))
            );
        }

        if (!string.IsNullOrEmpty(orderStatus) && orderStatus.ToLower() != "all")
        {
            orders = orders.Where(o => o.Orderstatus.ToLower() == orderStatus.ToLower());
        }

        DateTime currentDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        switch (dateRange?.ToLower())
        {
            case "last 7 days":
                orders = orders.Where(o => o.Createdat >= currentDate.AddDays(-7));
                break;
            case "last 30 days":
                orders = orders.Where(o => o.Createdat >= currentDate.AddDays(-30));
                break;
            case "current month":
                orders = orders.Where(o => o.Createdat.Year == currentDate.Year && o.Createdat.Month == currentDate.Month);
                break;
            case "all time":
            default:
                break;
        }

        if (!string.IsNullOrEmpty(sortOrder))
        {
            switch (sortOrder.ToLower())
            {
                case "date_asc":
                    orders = orders.OrderBy(o => o.Createdat);
                    break;
                case "date_desc":
                    orders = orders.OrderByDescending(o => o.Createdat);
                    break;
                case "customer_asc":
                    orders = orders.OrderBy(o => o.Customer != null ? o.Customer.Name : "");
                    break;
                case "customer_desc":
                    orders = orders.OrderByDescending(o => o.Customer != null ? o.Customer.Name : "");
                    break;
                case "status_asc":
                    orders = orders.OrderBy(o => o.Orderstatus);
                    break;
                case "status_desc":
                    orders = orders.OrderByDescending(o => o.Orderstatus);
                    break;
                case "amount_asc":
                    orders = orders.OrderBy(o => o.Totalamount);
                    break;
                case "amount_desc":
                    orders = orders.OrderByDescending(o => o.Totalamount);
                    break;
                case "order_asc":
                    orders = orders.OrderBy(o => o.Id);  // Ensuring Order by ID Ascending
                    break;
                case "order_desc":
                    orders = orders.OrderByDescending(o => o.Id);  // Ensuring Order by ID Descending
                    break;
                default:
                    orders = orders.OrderByDescending(o => o.Id);  // Default Sorting by ID Descending
                    break;
            }
        }
        else
        {
            orders = orders.OrderBy(o => o.Id);
        }

        if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
        {
            if (DateTime.TryParse(fromDate, out DateTime parsedFromDate) && DateTime.TryParse(toDate, out DateTime parsedToDate))
            {
                orders = orders.Where(o => o.Createdat >= parsedFromDate && o.Createdat <= parsedToDate);
            }
        }


        return orders.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(o => new OrdersExportDto
        {
            orderID = o.Id,
            Date = o.Createdat,
            CustomerName = o.Customer != null ? o.Customer.Name : "Unknown",
            Status = o.Orderstatus,
            PaymentMode = o.Payment != null ? o.Payment.Paymentmethod : "N/A",
            TotalAmount = o.Totalamount,
            feedbackDto = o.Feedbacks.Select(f => new FeedbackDto
            {
                avgRating = f.Avgrating
            }).ToList()
        }).ToList();
    }


    public int countOrders(string searchString, string orderStatus, string dateRange, string fromDate, string toDate)
    {
        IQueryable<Order> orders = context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Feedbacks)
            .Include(o => o.Payment)
            .AsQueryable();
        if (!string.IsNullOrEmpty(searchString))
        {
            string searchLower = searchString.ToLower();
            orders = orders.Where(o =>
                o.Id.ToString().Contains(searchLower) ||
                (o.Customer != null && o.Customer.Name.ToLower().Contains(searchLower))
            );
        }

        if (!string.IsNullOrEmpty(orderStatus) && orderStatus.ToLower() != "all")
        {
            orders = orders.Where(o => o.Orderstatus.ToLower() == orderStatus.ToLower());
        }


        DateTime currentDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        switch (dateRange?.ToLower())
        {
            case "last 7 days":
                orders = orders.Where(o => o.Createdat >= currentDate.AddDays(-7));
                break;
            case "last 30 days":
                orders = orders.Where(o => o.Createdat >= currentDate.AddDays(-30));
                break;
            case "current month":
                orders = orders.Where(o => o.Createdat.Year == currentDate.Year && o.Createdat.Month == currentDate.Month);
                break;
            case "all time":
            default:
                break;
        }

        if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
        {
            if (DateTime.TryParse(fromDate, out DateTime parsedFromDate) && DateTime.TryParse(toDate, out DateTime parsedToDate))
            {
                orders = orders.Where(o => o.Createdat >= parsedFromDate && o.Createdat <= parsedToDate);
            }
        }
        return orders.Count();
    }


    public List<OrdersExportDto> GetOrdersDetail()
    {
        return context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Feedbacks)
            .OrderBy(o => o.Id)
            .Select(o => new OrdersExportDto
            {
                orderID = o.Id,
                Date = o.Createdat,
                CustomerName = o.Customer.Name,
                Status = o.Orderstatus,
                PaymentMode = o.Payment != null ? o.Payment.Paymentmethod : null,
                TotalAmount = o.Totalamount,
                feedbackDto = o.Feedbacks.Select(f => new FeedbackDto
                {
                    avgRating = f.Avgrating
                }).ToList()
            })
            .ToList();
    }


    public List<OrdersExportDto> GetOrdersDetail(string searchString, string orderStatus, string dateRange)
    {
        var query = context.Orders.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(o => o.Customer.Name.Contains(searchString) || o.Id.ToString() == searchString);
        }

        if (!string.IsNullOrEmpty(orderStatus) && orderStatus != "All")
        {
            query = query.Where(o => o.Orderstatus == orderStatus);
        }

        if (dateRange != "All time")
        {
            DateTime startDate;
            DateTime endDate = DateTime.Today.AddDays(1).AddTicks(-1);
            DateTime currentDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
            switch (dateRange?.ToLower())
            {
                case "last 7 days":
                    query = query.Where(o => o.Createdat >= currentDate.AddDays(-7));
                    break;
                case "last 30 days":
                    query = query.Where(o => o.Createdat >= currentDate.AddDays(-30));
                    break;
                case "current month":
                    query = query.Where(o => o.Createdat.Year == currentDate.Year && o.Createdat.Month == currentDate.Month);
                    break;
                case "all time":
                default:
                    break;
            }
        }

        return query.Select(o => new OrdersExportDto
        {
            orderID = o.Id,
            Date = o.Createdat,
            CustomerName = o.Customer.Name,
            Status = o.Orderstatus,
            TotalAmount = o.Totalamount,
            PaymentMode = o.Payment.Paymentmethod,
            feedbackDto = o.Feedbacks.Select(f => new FeedbackDto
            {
                avgRating = f.Avgrating
            }).ToList(),
            Rating = (int)(o.Feedbacks.Any() ? o.Feedbacks.Average(f => f.Avgrating) : 0)

        }).ToList();
    }

    public OrderDetailsDto GetInvoice(int orderId)
    {

        Order orderDetail = context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Tableordermappings)
                .ThenInclude(ot => ot.Table)
                .ThenInclude(t => t.Section)
            .Include(o => o.Ordereditems)
                .ThenInclude(oi => oi.Menuitem)
            .Include(o => o.Ordereditems)
                .ThenInclude(oi => oi.Ordereditemmodifiermappings)
                .ThenInclude(oim => oim.Modifier)
            .Include(o => o.Ordertaxmappings)
                .ThenInclude(ot => ot.Tax)
            .Include(o => o.Payment)
            .Include(o => o.Invoices)
            .FirstOrDefault(u => u.Id == orderId) ?? new Order();


        var orderItems = orderDetail.Ordereditems.Select(oi => new OrderItemDto
        {
            ItemName = oi.Menuitem?.Name ?? "Unknown",
            Quantity = oi.Quantity,
            Price = oi.Menuitem?.Totalamountwithtax ?? 0,
            TotalAmount = oi.Quantity * (oi.Menuitem?.Totalamountwithtax ?? 0),
            ItemModifiers = oi.Ordereditemmodifiermappings.Select(oim => new OrderItemModifierDto
            {
                ModifierName = oim.Modifier?.Name ?? "Unknown",
                Quantity = oim.Quantityofmodifier ?? 0,
                Price = oim.Modifier?.Rate ?? 0,
                TotalAmount = (oim.Quantityofmodifier ?? 0) * (oim.Modifier?.Rate ?? 0)
            }).ToList()
        }).ToList();


        decimal subtotal = orderItems.Sum(i => i.TotalAmount + i.ItemModifiers.Sum(m => m.TotalAmount));

        var enabledTaxes = orderDetail.Ordertaxmappings
            .Where(t => t.Tax != null && t.Tax.Isactive == true)
            .Select(t => t.Tax)
            .ToList();

        Dictionary<string, decimal> taxSummary = new Dictionary<string, decimal>();
        decimal totalTaxAmount = 0;

        foreach (var tax in enabledTaxes)
        {
            decimal taxAmount = 0;
            decimal amount = subtotal * tax.Percentage ?? 0;
            if (tax.Type == true)
            {
                taxAmount = amount / 100;
            }
            else
            {
                taxAmount = tax.Taxvalue ?? 0;
            }

            if (taxSummary.ContainsKey(tax.Name))
            {
                taxSummary[tax.Name] += taxAmount;
            }
            else
            {
                taxSummary[tax.Name] = taxAmount;
            }

            totalTaxAmount += taxAmount;
        }

        List<OrderTaxDto> appliedTaxes = taxSummary.Select(t => new OrderTaxDto
        {
            TaxName = t.Key,
            TaxAmount = t.Value
        }).ToList();

        decimal totalAmount = subtotal + totalTaxAmount;
        
        
        OrderDetailsDto orderDetails = new OrderDetailsDto
        {
            PaymentMethod=orderDetail.Payment?.Paymentmethod,
            CustomerName = orderDetail.Customer?.Name ?? "Guest",
            CustomerEmail = orderDetail.Customer?.Email ?? "",
            CustomerPhone = orderDetail.Customer?.Phone.ToString() ?? "",
            OrderNo = orderDetail.Id,
            InvoiceNo = orderDetail.Id, 
            placedon = orderDetail.Createdat,
            Date = orderDetail.Payment?.Createdat,
            orderDuration =  orderDetail.Modifiedat - orderDetail.Createdat,
            SectionName = orderDetail.Tableordermappings.Select(t => t.Table?.Section?.Name).FirstOrDefault() ?? "N/A",
            TableName = orderDetail.Tableordermappings.Select(t => t.Table?.Name).FirstOrDefault() ?? "N/A",
            OrderItems = orderItems,
            Subtotal = subtotal,
            AppliedTaxes = appliedTaxes,
            TotalAmount = totalAmount
        };
        return orderDetails;
    }
}

