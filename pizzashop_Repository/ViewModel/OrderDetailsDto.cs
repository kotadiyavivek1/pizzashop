namespace pizzashop_Repository.ViewModel;
public class OrderDetailsDto
{
    public string? PaymentMethod{get; set;}
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }
    public int OrderNo { get; set; }
    public int InvoiceNo { get; set; }
    public DateTime? Date { get; set; }
    public DateTime? placedon {get; set;}
    public DateTime? modifiedon {get; set;}
    public TimeSpan? orderDuration {get; set;}

    public string? SectionName { get; set; }
    public string? TableName { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = new();
    public decimal Subtotal { get; set; }
    public List<OrderTaxDto> AppliedTaxes { get; set; } = new();
    public decimal TotalAmount { get; set; }

}

public class OrderItemDto
{
    public string ItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderItemModifierDto> ItemModifiers { get; set; } = new();
}

public class OrderItemModifierDto
{
    public string ModifierName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalAmount { get; set; }
}

public class OrderTaxDto
{
    public string TaxName { get; set; } = string.Empty;
    public decimal TaxAmount { get; set; }
}