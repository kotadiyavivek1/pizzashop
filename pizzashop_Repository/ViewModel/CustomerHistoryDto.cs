namespace pizzashop_Repository.ViewModel;
public class CustomerHistoryDto
{
    public string Name { get; set; } = null!;
    public decimal? AverageBill { get; set; }
    public string MobileNumber { get; set; } = null!;
    public string ComingSince { get; set; } = null!;
    public decimal? MaxOrder { get; set; }
    public int Visits { get; set; }
    public List<OrderDetailDto>? orderDetails{get; set;}
}

public class OrderDetailDto 
{
    public int OrderId { get; set;}
    public string? orderDate{get; set;}
    public string? OrderType{get; set;}
    public string? PaymentMethod{get; set;}
    public decimal Amount{get; set;}
    public int Items{get; set;}
}