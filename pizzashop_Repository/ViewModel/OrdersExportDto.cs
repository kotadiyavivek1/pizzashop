namespace pizzashop_Repository.ViewModel;
public class OrdersExportDto
{
    public int orderID{get; set;}
    public DateTime Date{get; set;}
    public string? CustomerName{get; set;} 
    public string?  Status{get; set;}
    public string? PaymentMode{get; set;}
    public int Rating{get; set;} 
    public decimal? TotalAmount{get; set;}
    public List<FeedbackDto>? feedbackDto{get; set;} = new List<FeedbackDto>();
}
public class FeedbackDto
{
    public int? avgRating{get; set;}
}