namespace pizzashop_Repository.ViewModel;
public class customerDetailsDto
{
    public int customerId{get; set;}
    public string? Name{get; set;}
    public string? Email{get; set;}
    public string? PhoneNumber{get; set;}
    public DateTime Date{get; set;}
    public int CountOrders{get; set;}
    public int Amount{get; set;}
}