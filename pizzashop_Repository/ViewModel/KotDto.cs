using System.Dynamic;

namespace pizzashop_Repository.ViewModel;
public class KotDto{
    public List<Category>? categories{get; set;}
    public string? Status{get; set;}
    public int OrderId{get; set;}
    public DateTime CreatedAt{get; set;}
    public string? SectionName{get; set;}
    public string? TableName{get; set;}
    public string? OrderInstruction{get; set;}
    public List<KotItemDTO>? KotItem { get; set; }
    public string? CategoryName{get; set;} 
}

public class KotItemDTO
{
    public int OrderedItemId{get; set;}
    public int ReadyItemQuantity{get; set;}
    
    public string KotItemName { get; set; } = null!;
    public int Quantity { get; set; }

    public string? ItemInstruction { get; set; }

    public List<KotItemModifierDTO>? Modifiers { get; set; }
}

public class KotItemModifierDTO
{
    public string KotItemModifierName { get; set; } = null!;
}

public class KotItemUpdateDto
{
    public int OrderedItemId{get;set;}
    public int Quantity{get; set;}
}
